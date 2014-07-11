<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Escc.SupportWithConfidence.Website.Search" %>
<%@ Register TagPrefix="SWC" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>
<%@ Register TagPrefix="SWC" TagName="Related" Src="~/Related.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
		<Egms:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="vetted; approved; home care; care services; Support with confidence"
			description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
			runat="server" />
    <Egms:Css runat="server" Files="FormsSmall;SupportWithConfidence" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <Egms:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <Egms:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
<div class="article">
<section>
  <swc:SearchControl id="SearchControls" runat="server" />
    <SWC:Related runat="server" />
</section>
</div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="javascript">
    <script src="Scripts/CategoryShowHide.js" type="text/javascript"></script>
    <script src="Scripts/button.js" type="text/javascript"></script>
</asp:Content>