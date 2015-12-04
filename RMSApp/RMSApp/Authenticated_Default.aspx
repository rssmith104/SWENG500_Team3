<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authenticated_Default.aspx.vb" Inherits="RMSApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
  <style>
  .carousel-inner > .item > img,
  .carousel-inner > .item > a > img {
      width: 40%;
      margin: auto;
  }
  </style>


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
        <a href="./Search">
            <img src="Images/cordonbleu.jpg" alt="Bleu" width="460" height="345">
        </a>
          
        <div class="carousel-caption">
          <h3>Search</h3>
          <p>Search through our library of user recipes for something new!</p>
        </div>
      </div>

      <div class="item">
          <a href="./CircleOfFriend">
            <img src="Images/Japanese_Salt_flavor_Sapporo_Ramen.JPG" alt="Ramen" width="460" height="345">
          </a>
        <div class="carousel-caption">
          <h3>Circles</h3>
          <p>Check out what people in your circles are posting!</p>
        </div>
      </div>
    
      <div class="item">
          <a href="./Account/ManageProfile">
              <img src="Images/vegetable-stock-recipe.jpg" alt="Vegetable" width="460" height="345">
          </a>
        <div class="carousel-caption">
          <h3>Manage your profile</h3>
          <p>Update your profile information!</p>
        </div>
      </div>

      <div class="item">
          <a href="./Account/AddRecipe">
              <img src="Images/stew-460x345.jpg" alt="BeefStew" width="460" height="345">
          </a>
        <div class="carousel-caption">
          <h3>Create a Recipe</h3>
          <p>Idea for a recipe? Why not create one!</p>
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

    <div class="row">
        <div class="col-md-4">
            <h2>Store Recipes</h2>
            <p>
                Store hundreds of your favorite recipes.  Share them with family and friends.  
            </p>
            <p>
                <a class="btn btn-primary btn-md" href="./Account/Register">Create An Account &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Search for Popular Recipes</h2>
            <p>
                Search the Recipe Management System Recipe archives for thousands of favorite recipes from either your own circle of friends or
                from other RMS users..
            </p>
            <p>
                <a class="btn btn-primary btn-md" href="./Account/Login">Already Have an Account &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Locate a Store Near You</h2>
            <p>
                Do not have an ingredient on-hand? RMS provides One-Click shopping directly from the recipe to the vendor for fast, accurate & convenient
                purchasing for those hard-to-find ingredients.  Who actually has "liquid smoke" in their pantry!  Really!
            </p>
            <p>
                <a class="btn btn-primary btn-md" href="./Account/Register">Let's Get Started &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
