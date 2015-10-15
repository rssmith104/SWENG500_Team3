<%@ Page Title="Welcome" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Welcome.aspx.vb" Inherits="RMSApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
  <style>
  .carousel-inner > .item > img,
  .carousel-inner > .item > a > img {
      width: 70%;
      margin: auto;
  }
  </style>

<div class="container">
  <br>
  <div id="myCarousel" class="carousel slide" data-ride="carousel">
    <!-- Indicators -->
    <ol class="carousel-indicators">
      <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
      <li data-target="#myCarousel" data-slide-to="1"></li>
      <li data-target="#myCarousel" data-slide-to="2"></li>
      <li data-target="#myCarousel" data-slide-to="3"></li>
    </ol>

    <!-- Wrapper for slides -->
    <div class="carousel-inner" role="listbox">

      <div class="item active">
        <img src="Images/cordonbleu.jpg" alt="Bleu" width="460" height="345">
        <div class="carousel-caption">
          <h3>Favorites</h3>
          <p>View your Favorite recipes!</p>
        </div>
      </div>

      <div class="item">
        <img src="Images/Japanese_Salt_flavor_Sapporo_Ramen.JPG" alt="Ramen" width="460" height="345">
        <div class="carousel-caption">
          <h3>Circles</h3>
          <p>Check out what people in your circles are posting!</p>
        </div>
      </div>
    
      <div class="item">
        <img src="Images/vegetable-stock-recipe.jpg" alt="Vegetable" width="460" height="345">
        <div class="carousel-caption">
          <h3>Top Rated</h3>
          <p>Enjoy some of the top rated recipes!</p>
        </div>
      </div>

      <div class="item">
        <img src="Images/stew-460x345.jpg" alt="BeefStew" width="460" height="345">
        <div class="carousel-caption">
          <h3>Create a Recipe</h3>
          <p>Idea for a recipe? Why not create one?</p>
        </div>
      </div>
  
    </div>

    <!-- Left and right controls -->
    <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
      <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
      <span class="sr-only">Previous</span>
    </a>
    <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
      <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
      <span class="sr-only">Next</span>
    </a>
  </div>
</div>

    <hr />

    <div class="row">
        <br />
        <table>
            <div class="col-md-1">

                <td id="left" style="float: left">
                    <img src="Images/Japanese_Salt_flavor_Sapporo_Ramen.JPG" style="width: 520px; height: 380px" />
                </td>
                <td id="right" style="float: right">
                    <br />
                    <h2>Sapporo Ramen</h2>
                    <p></p>
                </td>
            </div>
        </table>
    </div>
</asp:Content>