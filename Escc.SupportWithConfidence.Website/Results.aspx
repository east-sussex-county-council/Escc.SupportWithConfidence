<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Escc.SupportWithConfidence.Website.Results" %>
<%@ Register TagPrefix="SWC" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>
<%@ Register TagPrefix="SWC" TagName="Related" Src="~/Related.ascx" %>
<asp:Content id="Content1" runat="server" ContentPlaceHolderID="metadata">
		<Egms:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="vetted; approved; home care; care services; Support with confidence"
			description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
			runat="server" />
    <Egms:Css runat="server" Files="FormsSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <Egms:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <Egms:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content id="Content3" runat="server" ContentPlaceHolderID="content">
<div class="article">
    <section>
        <SWC:ResultControl id="results" runat="server" />
    </section>
</div>
</asp:Content>