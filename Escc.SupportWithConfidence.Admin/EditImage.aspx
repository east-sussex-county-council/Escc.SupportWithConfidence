<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditImage.aspx.cs" Inherits="Escc.SupportWithConfidence.Admin.EditImage" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
		<Metadata:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards;Consumer affairs;Fair trading"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="Buy with confidence; consumer; comments; trader; trading standards; business; shop; complaint"
			description="Find traders which are approved members of the Buy With Confidence scheme in East Sussex"
			runat="server" />
             <style type="text/css">
    .newsearch {float:right; padding-bottom:1em;}
    </style>
    <link href="/wres/css/forms-3.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script src="Scripts/jquery.Jcrop.min.js" type="text/javascript"></script>

<!--<script type="text/javascript">

    jQuery(document).ready(function () {

        jQuery('#ctl00_content_imgCrop').Jcrop({

            onSelect: storeCoords

        });

    });



    function storeCoords(c) {

        jQuery('#ctl00_content_X').val(c.x);

        jQuery('#ctl00_content_Y').val(c.y);

        jQuery('#ctl00_content_W').val(c.w);

        jQuery('#ctl00_content_H').val(c.h);

    };

 

</script>-->

    <ClientDependency:Css runat="server" Files="ContentSmall" Moveable="False"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>

</asp:Content>




<asp:Content runat="server" ContentPlaceHolderID="content">
  <div>
 
 <asp:ValidationSummary id="validationSummary" runat="server" showsummary="true" enableclientscript="false" displaymode="bulletlist" /></asp:validationSummary>

    <asp:Panel ID="pnlUpload" runat="server">

      <asp:FileUpload ID="Upload" runat="server" />

      <br />

      <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />

      <asp:Label ID="lblError" runat="server" Visible="false" />

    </asp:Panel>

    <asp:Panel ID="pnlCrop" runat="server" Visible="false">

      <asp:Image ID="imgCrop" runat="server" />

      <br />


      <asp:HiddenField ID="X" runat="server" value="10" />

      <asp:HiddenField ID="Y" runat="server"  value="10"/>

      <asp:HiddenField ID="W" runat="server" value="182" />

      <asp:HiddenField ID="H" runat="server" value="217" />

      <asp:Button ID="btnCrop" runat="server" Text="Crop" OnClick="btnCrop_Click" />

    </asp:Panel>

    <asp:Panel ID="pnlCropped" runat="server" Visible="false">

      <asp:Image ID="imgCropped" runat="server" />

       <asp:Button ID="btnSaveToDB" runat="server" Text="Save" OnClick="btnSaveToDB_Click" />
       <a href="Provider.aspx?ref=1">Return to provider record</a>
    </asp:Panel>

  </div>
</asp:Content>
