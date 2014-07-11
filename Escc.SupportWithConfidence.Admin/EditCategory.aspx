<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="Escc.SupportWithConfidence.Admin.EditCategory" %>

<%@ Register TagPrefix="SWC" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
	<Egms:MetadataControl id="headContent" title="Support with Confidence" 
		IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
			lgiltype="Applications for service" 
			lgtltype="Forms" 
			dateissued="2004-11-20" 
			keywords="vetted; approved; home care; care services; Support with confidence"
			description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
			runat="server" />

			<link rel="stylesheet" type="text/css" href="/wres/css/forms-3.css" />
</asp:Content>




<asp:Content runat="server" ContentPlaceHolderID="related">
		
</asp:Content>
    
<asp:Content runat="server" ContentPlaceHolderID="content">
 <swc:editcategorycontrol id="EditCategoryControl" runat="server" />
</asp:Content>
