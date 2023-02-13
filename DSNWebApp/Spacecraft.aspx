<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Spacecraft.aspx.cs" Inherits="DSNWebApp.WebForm2" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-md-4">

        <div class="justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">
                <%=launchVehicleName %></h1>
            <h2><%=launchVehicleOrbit %></h2>
            <h6><%=payloadName %> | <%=payloadType %></h6>
        </div>
        <div class="justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h3 class="h2">Send Command to Launch Vehicle</h3>
            <div class="my-1 col-2">
                <label class="mr-sm-2" for="commandLaunchVehicle">Command</label>
                <select class="custom-select mr-sm-2" id="Select2" name="D2" runat="server">
                    <option selected value="">Choose...</option>
                    <option value="DEPLOY_PAYLOAD">DeployPayload</option>
                    <option value="DEORBIT">Deorbit</option>
                    <option value="START_TELEMETRY">StartTelemetry</option>
                    <option value="STOP_TELEMETRY">StopTelemetry</option>
                </select>
            </div>
            <asp:Button ID="Button1" runat="server" class="btn btn-primary my-3 ml-3 mb-3" OnClick="Button1_Click" Text="Send" />
            <asp:Label ID="Label1" runat="server" Text="" CssClass="text-success d-block mb-2"></asp:Label>
            <asp:Label ID="Label2" runat="server" Text="" CssClass="text-danger d-block mb-2"></asp:Label>
        </div>
        <div class="justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h3 class="h2">Send Command to Payload</h3>
            <div class="my-1 col-2">
                <label class="mr-sm-2" for="commandPayload">Command</label>
                <select class="custom-select mr-sm-2" id="Select1" name="D1" runat="server">
                    <option selected value="">Choose...</option>
                    <option value="START_DATA">StartData</option>
                    <option value="STOP_DATA">StopData</option>
                    <option value="DECOMMISSION">Decomission</option>
                    <option value="START_TELEMETRY">StartTelemetry</option>
                    <option value="STOP_TELEMETRY">StopTelemetry</option>
                </select>
            </div>
            <asp:Button ID="Button2" class="btn btn-primary my-3 ml-3 mb-3" runat="server" OnClick="Button2_Click" Text="Send" />
            <asp:Label ID="Label3" runat="server" Text="" CssClass="text-success d-block mb-2"></asp:Label>
            <asp:Label ID="Label4" runat="server" Text="" CssClass="text-danger d-block mb-2"></asp:Label>
        </div>
        <div class="justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3">
            <h3 class="h2">Data
            </h3>
            <asp:Button ID="Button3" runat="server" Text="Refresh Data" CssClass="btn btn-primary my-3"/>
            <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="home-tab" data-toggle="tab" data-target="#active" type="button" role="tab" aria-controls="active" aria-selected="true">Launch Vehicle Telemetry Data</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="profile-tab" data-toggle="tab" data-target="#waiting" type="button" role="tab" aria-controls="waiting" aria-selected="false">Payload Telemetry Data</button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="transmission-tab" data-toggle="tab" data-target="#transmission" type="button" role="tab" aria-controls="transmission" aria-selected="false">Payload Transmission Data</button>
                </li>
            </ul>
            <div class="tab-content" id="myTabContent">
                <div class="tab-pane fade show active" id="active" role="tabpanel" aria-labelledby="active-tab">
                    <div class="list-group">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <a href="#" class="list-group-item list-group-item-action">
                                    <div class="d-flex w-100 justify-content-between">
                                        <p class="mb-1">altitude: <%#Eval("altitude")%></p>
                                        <small><%#Eval("createdDate")%></small>
                                    </div>
                                    <p class="mb-1">longitude: <%#Eval("longitude")%></p>
                                    <p class="mb-1">latitude: <%#Eval("latitude")%></p>
                                    <p class="mb-1">temperature: <%#Eval("temperature")%></p>
                                    <p class="mb-1">timeToOrbit: <%#Eval("timeToOrbit")%></p>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="tab-pane fade " id="waiting" role="tabpanel" aria-labelledby="waiting-tab">
                    <div class="list-group">
                        <asp:Repeater ID="Repeater2" runat="server">
                            <ItemTemplate>
                                <a href="#" class="list-group-item list-group-item-action">
                                    <div class="d-flex w-100 justify-content-between">
                                        <p class="mb-1">altitude: <%#Eval("altitude")%></p>
                                        <small><%#Eval("createdDate")%></small>
                                    </div>
                                    <p class="mb-1">longitude: <%#Eval("longitude")%></p>
                                    <p class="mb-1">latitude: <%#Eval("latitude")%></p>
                                    <p class="mb-1">temperature: <%#Eval("temperature")%></p>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="tab-pane fade " id="transmission" role="tabpanel" aria-labelledby="transmission-tab">
                    <div class="list-group">
                        <asp:Repeater ID="Repeater3" runat="server">
                            <ItemTemplate>

                                <a href="#" class="list-group-item list-group-item-action">

                                    <div class="d-flex w-100 justify-content-between">

                                        <small><%#Eval("createdDate")%></small>
                                    </div>
                                    <p class="mb-1">rainfall: <%#Eval("rainfall")%></p>
                                    <p class="mb-1">humidity: <%#Eval("humidity")%></p>
                                    <p class="mb-1">snow: <%#Eval("snow")%></p>

                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="Repeater4" runat="server">
                            <ItemTemplate>

                                <a href="#" class="list-group-item list-group-item-action">

                                    <div class="d-flex w-100 justify-content-between">

                                        <small><%#Eval("createdDate")%></small>
                                    </div>
                                    <p class="mb-1">uplink: <%#Eval("uplink")%></p>
                                    <p class="mb-1">downlink: <%#Eval("downlink")%></p>
                                    <p class="mb-1">transponders: <%#Eval("transponders")%></p>

                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="Repeater5" runat="server">
                            <ItemTemplate>
                                <a href="#" class="list-group-item list-group-item-action">

                                    <div class="d-flex w-100 justify-content-between">

                                        <small><%#Eval("createdDate")%></small>
                                    </div>
                                    <img src="<%#Eval("imageData")%>" alt="Image from payload">
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </main>
</asp:Content>
