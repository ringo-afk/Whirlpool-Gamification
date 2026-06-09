using UnityEngine.Networking;

public class ByPassCertificateJE : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}