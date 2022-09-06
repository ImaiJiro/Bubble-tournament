var libraryWebSockets = {
    $webSocketInstances: [],

    /**
     * @return {number}
     */
    socketCreate: function (url) {
        var str = Pointer_stringify(url);
        var socket = {
            socket: new WebSocket(str),
            buffer: new Uint8Array(0),
            error: null,
            messages: []
        };

        socket.socket.binaryType = 'arraybuffer';

        socket.socket.onmessage = function (e) {
            // Todo: handle other data types?
            if (typeof e.data === "string") { // instanceof String not work properly
                socket.messages.push(new TextEncoder("utf-8").encode(e.data));
            } else if (e.data instanceof Blob) {
                var reader = new FileReader();
                reader.addEventListener("loadend", function () {
                    var array = new Uint8Array(reader.result);
                    socket.messages.push(array);
                });
                reader.readAsArrayBuffer(e.data);
            } else if (e.data instanceof ArrayBuffer) {
                var array = new Uint8Array(e.data);
                socket.messages.push(array);
            }

        };

        socket.socket.onclose = function (e) {
            if (e.code !== 1000) {
                if (e.reason != null && e.reason.length > 0)
                    socket.error = e.reason;
                else {
                    switch (e.code) {
                        case 1001:
                            socket.error = "Endpoint going away.";
                            break;
                        case 1002:
                            socket.error = "Protocol error.";
                            break;
                        case 1003:
                            socket.error = "Unsupported message.";
                            break;
                        case 1005:
                            socket.error = "No status.";
                            break;
                        case 1006:
                            socket.error = "Abnormal disconnection.";
                            break;
                        case 1009:
                            socket.error = "Data frame too large.";
                            break;
                        default:
                            socket.error = "Error " + e.code;
                    }
                }
            }
        };
        return webSocketInstances.push(socket) - 1;
    },

    /**
     * @return {number}
     */
    socketState: function (s) {
        var socket = webSocketInstances[s];
        return socket.socket.readyState;
    },

    /**
     * @return {number}
     */
    socketError: function (s, ptr, bufsize) {
        var socket = webSocketInstances[s];

        if (socket.error == null)
            return 0;

        var str = socket.error.slice(0, Math.max(0, bufsize - 1));
        stringToUTF8(str, ptr, str.length + 1);
        return 1;
    },

    socketSendBuff: function (s, ptr, length) {
        var socket = webSocketInstances[s];
        socket.socket.send(HEAPU8.buffer.slice(ptr, ptr + length));
    },

    socketSendStr: function (s, str) {
        var str_ = Pointer_stringify(str);
        var socket = webSocketInstances[s];
        socket.socket.send(str_);
    },

    /**
     * @return {number}
     */
    socketRecvLength: function (s) {
        var socket = webSocketInstances[s];

        if (socket.messages.length === 0)
            return 0;

        return socket.messages[0].length;
    },

    /**
     * @return {number}
     */
    socketRecv: function (s, ptr, length) {
        var socket = webSocketInstances[s];

        if (socket.messages.length === 0)
            return 0;

        if (socket.messages[0].length > length)
            return 0;

        HEAPU8.set(socket.messages[0], ptr);
        socket.messages = socket.messages.slice(1);
    },

    socketClose: function (s) {
        var socket = webSocketInstances[s];
        socket.socket.close();
    }
};

autoAddDeps(libraryWebSockets, '$webSocketInstances');
mergeInto(LibraryManager.library, libraryWebSockets);