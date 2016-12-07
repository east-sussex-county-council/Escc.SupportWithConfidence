<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Escc.SupportWithConfidence.Website.Search" %>
<%@ Register TagPrefix="SWC" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>
<%@ Register TagPrefix="SWC" TagName="Related" Src="~/Related.ascx" %>
<%@ Register TagPrefix="EastSussexGovUK" tagName="EastSussex1Space" src="~/1space.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="vetted; approved; home care; care services; Support with confidence"
			description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
            rssfeedtitle="Help at home you can trust"
			runat="server" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="FormsSmall;SupportWithConfidence" Moveable="False" />
        <ClientDependency:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
<div class="article">
<section>
  <swc:SearchControl id="SearchControls" runat="server" />
</section>
</div>
<SWC:Related runat="server" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="javascript">
    <ClientDependency:Script Files="SupportWithConfidenceCategories;SupportWithConfidenceEnterFix" runat="server" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="afterForm">
    <EastSussexGovUK:EastSussex1Space runat="server" />
</asp:Content>
