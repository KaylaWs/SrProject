<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="System.IO"%>
<%@ Import Namespace="NaturalLanguageProcessingCSharp"%>

<!DOCTYPE html>
<script runat="server">

    void Page_Load(Object sender, EventArgs e)
    {
        
        EntityExtractor seeker = new EntityExtractor();
        String line;
        try
        {
            using (StreamReader sr = new StreamReader(MapPath("TestCases/testText2.txt")))
            {
                line = sr.ReadToEnd();
                Console.WriteLine(line);
                Response.Write(line);
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(ex.Message);
            line = "";
        }
        List<String> ppl = seeker.ExtractEntities(line, EntityExtractor.EntityType.Person);
        foreach(String s in ppl)
        {
            Response.Write("/n");
            Response.Write(s);
        }
        /*List<String> org = seeker.ExtractEntities(line, EntityExtractor.EntityType.Organization);
        foreach (String s in org)
        {
            Response.Write(s);
        }
        List<String> loc = seeker.ExtractEntities(line, EntityExtractor.EntityType.Location);
        foreach (String s in loc)
        {
            Response.Write(s);
        }
        List<String> mon = seeker.ExtractEntities(line, EntityExtractor.EntityType.Money);
        foreach (String s in mon)
        {
            Response.Write(s);
        }
        List<String> tm = seeker.ExtractEntities(line, EntityExtractor.EntityType.Time);
        foreach (String s in tm)
        {
            Response.Write(s);
        }
        List<String> dt = seeker.ExtractEntities(line, EntityExtractor.EntityType.Date);
        foreach (String s in dt)
        {
            Response.Write(s);
        }*/
    }

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <p>

        </p>
    </div>
    </form>
</body>
</html>
