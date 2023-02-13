<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendCommand.aspx.cs" Inherits="DSNWebApp.WebForm1" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-md-4">

        <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">Send Command</h1>

        </div>
        <div class="list-group ">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <a href="/Spacecraft/?id=<%#Eval("id")%>" class="list-group-item d-flex justify-content-between lh-condensed">
                        <div>
                            <h6 class="my-0"><%#Eval("name")%></h6>
                            <small class="text-muted"><%#Eval("status")%></small>
                        </div>
                        <span class="text-muted">
                            <p><%#Eval("orbit")%></p>

                        </span>
                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </main>

</asp:Content>
