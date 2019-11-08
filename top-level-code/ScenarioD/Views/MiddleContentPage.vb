<?xml version="1.0" encoding="UTF-8"?>
<html>
    <head>
        <title><%= ViewData!Title %></title>
    </head>
    <body>
        <%= RenderSection(Of NavLinks)() %>

        <h1><%= ViewData!Message %></h1>

        <%= GetContent().Root %>

        <%= RenderSection(Of NavLinks)() %>
    </body>
</html>

<CompilerServices.DefaultOverrideMethod>
Public Overridable Function GetContent() As XDocument
    Return Nothing
End Function