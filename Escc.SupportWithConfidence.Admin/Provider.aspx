<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Provider.aspx.cs" Inherits="Escc.SupportWithConfidence.Admin.Provider" %>
<%@ Register TagPrefix="SWC" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
		<Metadata:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="vetted; approved; home care; care services; Support with confidence"
			description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
			runat="server" />
        <%--<EastSussexGovUK:ContextContainer runat="server" Legacy="true">
             <style type="text/css">
  #ctl00_content_editprovider_imageuploadercontrol
{clear:both; float:right; margin-left:30px; }
 .bwclogo {display:block;}
    </style>
    <link href="/wres/css/forms-3.css" rel="stylesheet" type="text/css" />
    </EastSussexGovUK:ContextContainer>
    <EastSussexGovUK:ContextContainer runat="server" Legacy="false">
        <Egms:Css runat="server" Files="FormsSmall" />
        <style>
        .imageupload { float: right; margin: 0 0 1.384615em 1.384615em; }
        </style>
    </EastSussexGovUK:ContextContainer>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <Egms:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <Egms:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer> --%>
</asp:Content>




    
<asp:Content runat="server" ContentPlaceHolderID="content">
<div class="full-page">
    <section>
      <swc:providerdetaileditcontrol id="editprovider" runat="server" />
    </section>
</div>
</asp:Content>
   
 
<asp:Content runat="server" ContentPlaceHolderID="supporting" />