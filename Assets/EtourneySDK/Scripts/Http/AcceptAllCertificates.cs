using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;

namespace Etourney.Scripts.Http
{
    internal class AcceptAllCertificates : CertificateHandler
    {
        private readonly string _publicKey = "3082010A0282010100C89333EC1CDCF8CAFCB2D2DC6740DCD68C26114428BC6DB75C7D3E1D4EAFD0B386D3ADDF27F8296EBA239E827FA250CB524BFDC6E49BEA0EAE802A397955C2CF889871FDF7CD2F71E394AB5148BB2C454DB022D7CEB4C2A0D8BC0407B35730F6C50A3E80A4F59EBF8781DAC94752FF152FF396E7D8AE5FF2D9E711BB7AD2FEE075BDD682D19BBA5C879F9ED6151793B3CD64F602C69DC0D19B7D7D7E1805C8F2A830B5248E7BE8813B0A5719BDACFC6875D0C1D54746D33AD8DEC4CEBA10C6A5904D3C0571CED54A739B2DEEB2ECD582C1AE5A7A161A15282910AB70C1AF55417E73827F3A1D650EDB4223B839940B5A7764AD3AA047E2B13775081D4D5E25E10203010001";
        
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateData);
            string incomingPublicKey = certificate.GetPublicKeyString();

            if (string.IsNullOrEmpty(incomingPublicKey))
                return false;
            
            if (incomingPublicKey.ToLower().Equals(_publicKey.ToLower()))
                return true;
            
            return false;
        }
    }
}