<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DSNWebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-md-4">
        <div class="chartjs-size-monitor">
            <div class="chartjs-size-monitor-expand">
            </div>
            <div class="chartjs-size-monitor-shrink">
                <div class=""></div>
            </div>
        </div>
        <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">Spacecraft Data</h1>

        </div>
        <ul class="nav nav-tabs" id="myTab" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="home-tab" data-toggle="tab" data-target="#active" type="button" role="tab" aria-controls="active" aria-selected="true">Active</button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="profile-tab" data-toggle="tab" data-target="#waiting" type="button" role="tab" aria-controls="waiting" aria-selected="false">Waiting</button>
            </li>
        </ul>
        <div class="tab-content" id="myTabContent">
            <div class="tab-pane fade show active" id="active" role="tabpanel" aria-labelledby="active-tab">
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
            </div>
            <div class="tab-pane fade" id="waiting" role="tabpanel" aria-labelledby="waiting-tab">
                <div class="list-group ">
                    <asp:Repeater ID="Repeater2" runat="server">
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
            </div>
        </div>
    </main>



</asp:Content>
