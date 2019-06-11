<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GeneradorDeCuestionariosJSON._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Modifique esta plantilla para poner en marcha su aplicación ASP.NET.</h2>
            </hgroup>
            <p>
                Para obtener más información sobre ASP.NET, visite <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                La página incluye <mark>vídeos, cursos y ejemplos</mark> para ayudarle a sacar el máximo partido a ASP.NET.
                Si tiene alguna pregunta relacionada con ASP.NET, visite
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">nuestros foros</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <br />
    GENERADOR DE ESTRUCTURA JSON PARA CUESTIONARIOS<br />
    Wizard of generation of JSON questionnaires<br />
    <asp:Panel ID="Panel1" runat="server">
        <span class="auto-style1">QUESTIONNAIRE DATA</span><br /> Name:
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        Extended Name:
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        Concept:
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button2" runat="server" Height="33px" OnClick="Button2_Click" Text="Save &amp; Continue" Width="202px" />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel2" runat="server" Visible="False">
        <span class="auto-style1">QUESTIONNAIRE SECTIONS</span><br /> Name Section:
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        (Example Header, Footer or Questions)<br /> Instructions:
        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        <asp:Button ID="Button13" runat="server" Height="34px" OnClick="Button13_Click" Text="Add" />
        <br />
        <br />
        <asp:ListBox ID="ListBox3" runat="server" Width="282px"></asp:ListBox>
        <br />
        <br />
        <asp:Button ID="Button3" runat="server" Height="34px" OnClick="Button3_Click" Text="Save &amp; Continue" />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel3" runat="server" Visible="False">
        <span class="auto-style1">Elements of this Section (Simple Text or Questions+Answers):<br />
        <br />
        </span><span>Section: </span>
        <asp:DropDownList ID="DropDownList2" runat="server">
        </asp:DropDownList>
        <br />
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem Value="text">Explanation Text</asp:ListItem>
            <asp:ListItem Value="question_answers">Question and Answers</asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        <br />
        <br />
        If the choice was &quot;explanation text&quot; left this box without value. If the choice was &quot;Questions and Answers&quot; Complete the answers.<br /> Answer:
        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
        &nbsp;<asp:Button ID="Button1" runat="server" Height="32px" Text="Add Answer" Width="113px" OnClick="Button1_Click" />
        <br />
        <asp:ListBox ID="ListBox1" runat="server"  Width="316px"></asp:ListBox>
        <br />
        (Double click to remove)<br />
        <br />
        <asp:Button ID="Button4" runat="server" Height="34px" Text="Save &amp; Add Another" Width="206px" />
        <asp:Button ID="Button5" runat="server" Height="33px" OnClick="Button5_Click" Text="Save &amp; Continue" />
        <asp:Button ID="Button14" runat="server" OnClick="Button14_Click" Text="Save &amp; Add More Sections" />
        <br />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel4" runat="server" Visible="False">
        <span class="auto-style1">SCORES</span><br /> Metric Score:
        <asp:TextBox ID="TextBox8" runat="server" Width="399px"></asp:TextBox>
        &nbsp;<asp:Button ID="Button6" runat="server" Height="31px" OnClick="Button6_Click" Text="Save" />
        <br />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel5" runat="server" Visible="False">
        <span class="auto-style1">SCORE DEVOLUTIONS &amp; CLASIFICATION<br /> </span><span>Minimum Score </span>
        <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
        <br />
        Maximum Score
        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
        <br />
        Message
        <asp:TextBox ID="TextBox11" runat="server" Width="303px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button7" runat="server" Height="36px" Text="Save &amp; Add Another" />
        <asp:Button ID="Button8" runat="server" Height="32px" OnClick="Button8_Click" Text="Save &amp; Continue" />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel6" runat="server" Visible="False">
        SCORE DOMAINS<br />
        <br />
        Domain Name:
        <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
        <br />
        Maximum Score Domain:
        <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button9" runat="server" Height="35px" OnClick="Button9_Click" Text="Save" />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel7" runat="server" Visible="False">
        DOMAIN QUESTIONS<br />&nbsp;&nbsp;
        <br />
        &nbsp;Domain Name
        <asp:DropDownList ID="DropDownList3" runat="server">
        </asp:DropDownList>
        <br />
        Domain Question
        <asp:DropDownList ID="DropDownList4" runat="server">
        </asp:DropDownList>
        &nbsp;<asp:Button ID="Button10" runat="server" Height="31px" Text="Add" />
        <br />
        <br />
        <asp:ListBox ID="ListBox2" runat="server" Width="254px"></asp:ListBox>
        <br />
        (Double click on a question to remove it)<br />
        <br />
        <asp:Button ID="Button11" runat="server" Height="32px" OnClick="Button11_Click" Text="Save &amp; Continue" />
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel8" runat="server" Visible="False">
        QUESTIONNAIRE FOOTER<br />
        <br />
        Text at the footer:
        <asp:TextBox ID="TextBox14" runat="server" Width="470px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button12" runat="server" Height="35px" OnClick="Button12_Click" Text="Save &amp; Finish" />
    </asp:Panel>
    <br />
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style1 {
            text-decoration: underline;
        }
    </style>
</asp:Content>

