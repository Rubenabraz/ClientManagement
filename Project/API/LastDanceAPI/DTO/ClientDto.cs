namespace LastDanceAPI.DTO;
public class ClientDto
{
    public string cltName { get; set; }
    public string cltSurname { get; set; }
    public string cltEmail { get; set; }
    public string cltPhoneNumber { get; set; }
    public string cltGender { get; set; }
}

public class ClientUpdateDto : ClientDto
{
    public bool cltActive { get; set; } = true;
    public string cltStatus { get; set; } = "ativo";
}

public class ClientDeleteDto : ClientUpdateDto
{
    public int cltID { get; set; }
    public string cltStatus { get; set; } = "removido";
}

