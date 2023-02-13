<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LaunchSpacecraft.aspx.cs" Inherits="DSNWebApp.LaunchSpacecraft" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-md-4">

        <div class="justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h2>New Spacecraft</h2>
        </div>
        <div class="form-group">
            <label for="exampleFormControlFile1">Launch Vehicle Configuration File</label>
            <asp:FileUpload ID="FileUpload1" runat="server" />
        </div>
        <div class="form-group">
            <label for="exampleFormControlFile2">Payload Configuration File</label>
            <asp:FileUpload ID="FileUpload2" runat="server" />
        </div>
        <asp:Button ID="Button2" class="btn btn-primary mb-2" runat="server" Text="Launch" OnClick="Button2_Click" />
        <br />
        <asp:Label ID="SuccessLabel" runat="server" Text="" CssClass="mt-2 text-success"></asp:Label>
        <asp:Label ID="FailedLabel" runat="server" Text="" CssClass="mt-2 text-danger"></asp:Label>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <br />
       
    </main>
</asp:Content>
