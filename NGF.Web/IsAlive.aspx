<%@ Page Language="C#"%>

<script language="C#" runat=server>
	void Page_Load(object sender, EventArgs e)
	{
        Response.Write(DateTime.Now.ToString());
        Response.End();
	}
</script>