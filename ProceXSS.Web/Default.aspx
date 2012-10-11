<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ProceXSS.Web._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
      for test javascript:alert(&quot;XSS&quot;)
    </p>
    <textarea id="txtXss" name="txtXss" cols="60" rows="20">  
    </textarea>
    <input id="btnSubmit" type="submit" value="Submit" />
</asp:Content>
