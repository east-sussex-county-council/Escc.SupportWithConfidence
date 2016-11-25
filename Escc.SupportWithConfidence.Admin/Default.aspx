<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Escc.SupportWithConfidence.Admin._Default" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl id="headContent" title="Manage Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
		lgiltype="Applications for service" 
		lgtltype="Forms" 
		dateissued="2004-11-20" 
		keywords="vetted; approved; home care; care services; Support with confidence"
		description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
		runat="server" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <div class="content text-content">
            <h1>Manage Support with Confidence</h1>
            <ul>
            <li><a href="categories.aspx">Manage categories</a></li>
            <li><a href="providers.aspx">Update a provider's information</a></li>
            </ul>
        </div>
    </div>
</asp:Content>


