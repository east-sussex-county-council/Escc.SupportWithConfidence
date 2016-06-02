<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Escc.SupportWithConfidence.Website.Results" %>
<%@ Register TagPrefix="SWC" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>
<%@ Register TagPrefix="SWC" TagName="Related" Src="~/Related.ascx" %>
<asp:Content id="Content1" runat="server" ContentPlaceHolderID="metadata">
		<Metadata:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="vetted; approved; home care; care services; Support with confidence"
			description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
			runat="server" />
    <ClientDependency:Css runat="server" Files="ContentSmall;FormsSmall" Moveable="False"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium;FormsMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge;FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content id="Content3" runat="server" ContentPlaceHolderID="content">
<div class="article">
    <section>
        <SWC:ResultControl id="results" runat="server" />
    </section>
</div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="afterForm">
    <SWC:Related runat="server"/>
    <EastSussexGovUK:EastSussex1Space runat="server" />
</asp:Content>