<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Providers.aspx.cs" Inherits="Escc.SupportWithConfidence.Admin.Providers" %>
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
    <ClientDependency:Css runat="server" Files="ContentSmall" Moveable="False"/>
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="ContentMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="ContentLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
<div class="full-page">
<section>
    <div class="content text-content">
      <swc:adminproviderresultcontrol id="Adminproviderresultcontrol1" runat="server" />
    </div>
</section>
</div>
</asp:Content>
 