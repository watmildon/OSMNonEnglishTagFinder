
public class TagInfoData
{
    public string url { get; set; }
    public DateTime data_until { get; set; }
    public int total { get; set; }
    public Datum[] data { get; set; }
}

public class Datum
{
    public string type { get; set; }
    public int count { get; set; }
    public float count_fraction { get; set; }
    public int values { get; set; }
}
