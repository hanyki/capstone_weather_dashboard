﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="WeatherStation" %>
<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Weather Incidents
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/demo/Content/WeatherIncident.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>

    <link href="/demo/Content/jquery-ui-1.7.2.custom.css" rel="stylesheet" type="text/css" />

    <script src="/demo/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <script src="http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"
        type="text/javascript"></script>

    <script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"></script>

    <script src="/demo/Scripts/WeatherIncident.js" type="text/javascript"></script>

    <script type="text/javascript">
        // NCDC Weather Incidents

        urls.push('<%= Url.Action("Index", "NcdcWeatherIncident", new { radius = ViewData["radius"], state = ViewData["state"], county = ViewData["county"], incidentFilter = ViewData["incidentFilter"], startDate = ((DateTime)ViewData["startDate"]).ToShortDateString(), endDate = ((DateTime)ViewData["endDate"]).ToShortDateString() }) %>');

        // Weather Underground Incidents
        <% for(DateTime d = (DateTime)ViewData["startDate"]; d <= (DateTime)ViewData["endDate"]; d = d.AddDays(1))
          {%>
            urls.push('<%= Url.Action("Index", "WeatherUndergroundWeatherIncident", new { date = d.ToShortDateString(), radius = ViewData["radius"], airportCode = ViewData["airportCode"], filter = ViewData["incidentFilter"] }) %>');
        <%}%>
        
        var incidentFound = false;
        var latitude = <%= ViewData["latitude"] %>;
        var longitude = <%= ViewData["longitude"] %>;     
        var homeAddress = '<%= ViewData["homeAddress"] %>';
        var html = '<%= ViewData["searchDataHtml"] %>';
    </script>

</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="search">
        <h2 id="searchString">
            Search Results</h2>
        <table>
            <%= ViewData["searchDataHtml"] %>
        </table>
        <div style="float: right;">
            <form id="allIncidentsPdf" action="/demo/IncidentPdf/AllIncidentsAsPdf" method="post" enctype="multipart/form-data">
                <input type="submit" value="Save All Incidents to PDF" style="float: right;" />
            </form>
            <div style="clear: right;"></div>
            <form id="allIncidentsEmail" method="post" style="margin-top: 10px;" enctype="multipart/form-data">
                <label for="e">Email Addresses (Separated by commas): </label>
                <input type="text" id="e" name="e" style="width:300px;" />
                <input type="submit" value="Email All Incidents" />
            </form>
        </div>
        <div style="clear: right;"></div>
    </div>
    <div id="progress">
        <p>
            <img src="/demo/Content/images/ajax-loader.gif" alt="Loading..." style="margin-top: 10px;" />
        </p>
        <p id="percent">
        </p>
    </div>
    <div id="results">
    </div>
</asp:Content>
