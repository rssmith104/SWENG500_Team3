﻿<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="RMSApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="background-image: url('<%= imgPath %>')">
        <h1 style="color:black">WELCOME TO THE RECIPE MANAGEMENT SYSTEM</h1>
        <p class="lead" style="color:white">Let's get started.  If you already have an account, Log In by selecting the Log In Link above.  Otherwise, let's create an account
                        and start accessing millions of recipes.</p>
        <p><a href="./Account/Register" class="btn btn-primary btn-lg">Create An Account &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Store Recipes</h2>
            <p>
                Store hundreds of you family favorite recipes.  Share them with family.  
            </p>
            <p>
                <a class="btn btn-default" href="./Account/Register">Create An Account &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Search for Popular Recipes</h2>
            <p>
                Search the Recipe Management System Recipe archives for thousands of favorite recipes from either your own circle of friends or
                from other RMS users..
            </p>
            <p>
                <a class="btn btn-default" href="./Account/Login">Already Have an Account &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Online Purchasing</h2>
            <p>
                Do not have an ingredient on-hand? RMS provides One-Click shopping directly from the recipe to the vendor for fast, accruate & convenient
                purchasing for those hard-to-find ingredients.  Who actually has "liquid smoke" in their pantry!  Really!
            </p>
            <p>
                <a class="btn btn-default" href="./Account/Register">Let's Get Started &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
