<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Escc.SupportWithConfidence.Website.Detail" %>
<%@ Register TagPrefix="SWC" TagName="Related" Src="~/Related.ascx" %>
<%@ Register TagPrefix="EastSussexGovUK" tagName="EastSussex1Space" src="~/1space.ascx" %>
<%@ Register TagPrefix="swc" Namespace="Escc.SupportWithConfidence.Controls" Assembly="Escc.SupportWithConfidence.Controls" %>

<asp:Content id="Content1" runat="server" ContentPlaceHolderID="metadata">
	<Metadata:MetadataControl id="headContent" title="Support with Confidence" 
	    IpsvPreferredTerms="Trading standards; Consumer protection; Carer support; Health and social care professionals; Care for the disabled; Home care; Care for the elderly; Day care; Care plans"
		lgiltype="Applications for service" 
		lgtltype="Forms" 
		dateissued="2004-11-20" 
		keywords="vetted; approved; home care; care services; Support with confidence"
		description="Find providers which are approved members of the Support with Confidence scheme in East Sussex"
		runat="server" />
</asp:Content>

   
<asp:Content id="Content3" runat="server" ContentPlaceHolderID="content">
    <div class="article">
    <article>
        <div class="content text-content">
            <swc:providerdetailcontrol Id="detail" runat="server"/>
        </div>
    </article>
    </div>
    <SWC:Related runat="server" />
    <EastSussexGovUK:EastSussex1Space runat="server"/>
</asp:Content>