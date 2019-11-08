Imports <xmlns="clr-namespace:WebViews">

<?xml version="1.0" encoding="UTF-8"?>
<html>
    <head>
        <meta charset="utf-8"/>
        <meta name="viewport" content="width=device-width, initial-scale=1"/>
        <meta name="google" content="notranslate"/>
        <style>
          a.btn { color: black; background: lightgray; display: block; }
          a.btn:hover { color: white; background: gray; }
        </style>
        <title><%= ViewData!Title %></title>
    </head>
    <body>
        <menu alwaysOnTop=”true”>
            <menuItem header="Declarations">
                <menuItem header="Types">
                    <menuItem header="Class" url="/declarations/types#class"/>
                    <menuItem header="Structure" url="/declarations/types#class"/>
                    <menuItem header="Interface" url="/declarations/types#class"/>
                    <menuItem header="Module" url="/declarations/types#class"/>
                    <menuItem header="Delegate" url="/declarations/types#class"/>
                </menuItem>
                <menuItem header="Members" url="/declarations/members"/>
            </menuItem>
            <menuItem header="Statements">
                <menuItem header="Variable Declarations" url="/locals"/>
                <menuItem header="Control Flow (If, Select Case)..." url="/control"/>
                <menuItem header="Loops (For, For Each, Do, While)..." url="/loops"/>
                <menuItem header="Other..." url="/statements#other"/>
            </menuItem>
            <menuItem header="Expressions" url="/expressions"/>
        </menu>

        <expander header="Carousel Example">
            <carousel itemsSource=<%= Database.GetCarouselData() %>>
                <carousel.itemTemplate>
                    <div>
                        <img src="{binding BackgroundImageUrl}" alt="{binding BackgroundImageAltText}"/>
                        <div class="carousel-caption">
                            <p>
                                <h3 content="{binding BackgroundImageAltText}"/>
                                <span content="{binding Description}"/>
                                <a class="btn btn-default" href="{binding DestinationUrl}">Learn More</a>
                            </p>
                        </div>
                    </div>
                </carousel.itemTemplate>
            </carousel>
        </expander>

        <h1><%= ViewData!Message %></h1>

        <expander header="Section 1">
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque posuere, orci non accumsan semper, arcu turpis auctor risus, lobortis elementum leo felis vel felis. Nulla sapien odio, ultricies et sapien vitae, maximus dignissim ligula. Praesent nec dapibus purus. Aenean vel nulla orci. Donec nisl neque, hendrerit nec magna vitae, semper vestibulum augue. Nulla augue ipsum, tincidunt sed cursus sed, varius vitae eros. Aenean mollis hendrerit ipsum, et iaculis nisi blandit ac. Morbi vitae diam ex. Interdum et malesuada fames ac ante ipsum primis in faucibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Praesent gravida purus a accumsan cursus.            </p>
            <p>Vestibulum ac massa et felis mattis imperdiet. Proin at ligula hendrerit, luctus erat id, gravida nibh. Cras hendrerit placerat lacus, quis maximus enim scelerisque eget. In sollicitudin neque a tellus interdum cursus. In hac habitasse platea dictumst. Vestibulum aliquam pulvinar velit vel maximus. Curabitur consectetur egestas scelerisque. Ut dictum dolor eu odio volutpat blandit. Proin lorem turpis, euismod varius massa vitae, interdum fermentum tortor. Aliquam ut felis et magna rhoncus egestas in lobortis enim. Integer imperdiet lobortis quam aliquam porta. Curabitur quis est eget lorem sagittis vehicula.</p>
            <p>In semper, magna a finibus dapibus, odio tortor dictum quam, in gravida ante ante at ante. Duis ac elementum augue, vehicula consectetur massa. Phasellus vulputate metus vitae mi tempus volutpat. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Pellentesque erat metus, lacinia in massa vel, tincidunt blandit turpis. Phasellus ac elit fermentum ipsum pharetra auctor id vel magna. Vivamus a urna venenatis, tempor nisl in, luctus est. Nullam ullamcorper, turpis aliquam accumsan malesuada, purus eros condimentum eros, eget faucibus felis ligula a nunc. Aenean sollicitudin sed nulla et convallis.</p>
            <p>Donec ut ante vel elit accumsan hendrerit nec et urna. Duis et dolor aliquet, sollicitudin ligula eget, iaculis eros. Quisque gravida nisl in sapien suscipit, non efficitur risus pulvinar. Maecenas blandit diam nunc. Sed ac venenatis justo, efficitur accumsan elit. In dictum felis mi, id malesuada ante accumsan at. Vivamus in velit vitae velit placerat gravida ac eget mi.</p>
            <p>Vivamus eget metus eget urna consectetur tristique. Maecenas egestas pretium enim, vel facilisis ante pellentesque ac. Nam maximus sapien nec velit efficitur, at pretium nulla maximus. Phasellus dui quam, commodo vitae pharetra sit amet, viverra a purus. Integer vitae justo sed justo malesuada pretium at sed nibh. Ut semper erat non cursus lobortis. Etiam volutpat magna nec tellus commodo blandit quis ac neque. Duis et dui vitae ex bibendum euismod. Duis ac placerat erat. Curabitur ipsum sem, dapibus nec fringilla quis, mattis sed diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Cras placerat, massa cursus vulputate hendrerit, purus felis dignissim diam, pellentesque ultrices elit ipsum id dolor. Praesent ac sem vitae felis elementum mollis vel ultrices eros. Nullam ut odio lacinia, molestie lorem a, porttitor nibh. Aenean et diam non arcu porta vehicula at ornare libero. Aliquam odio urna, fermentum sed faucibus nec, dapibus in felis.</p>
        </expander>
        <br/>
        <expander>
            <expander.header>
                <span style="font-weight: bold;">Section 2</span>
            </expander.header>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque posuere, orci non accumsan semper, arcu turpis auctor risus, lobortis elementum leo felis vel felis. Nulla sapien odio, ultricies et sapien vitae, maximus dignissim ligula. Praesent nec dapibus purus. Aenean vel nulla orci. Donec nisl neque, hendrerit nec magna vitae, semper vestibulum augue. Nulla augue ipsum, tincidunt sed cursus sed, varius vitae eros. Aenean mollis hendrerit ipsum, et iaculis nisi blandit ac. Morbi vitae diam ex. Interdum et malesuada fames ac ante ipsum primis in faucibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Praesent gravida purus a accumsan cursus.            </p>
            <p>Vestibulum ac massa et felis mattis imperdiet. Proin at ligula hendrerit, luctus erat id, gravida nibh. Cras hendrerit placerat lacus, quis maximus enim scelerisque eget. In sollicitudin neque a tellus interdum cursus. In hac habitasse platea dictumst. Vestibulum aliquam pulvinar velit vel maximus. Curabitur consectetur egestas scelerisque. Ut dictum dolor eu odio volutpat blandit. Proin lorem turpis, euismod varius massa vitae, interdum fermentum tortor. Aliquam ut felis et magna rhoncus egestas in lobortis enim. Integer imperdiet lobortis quam aliquam porta. Curabitur quis est eget lorem sagittis vehicula.</p>
            <p>In semper, magna a finibus dapibus, odio tortor dictum quam, in gravida ante ante at ante. Duis ac elementum augue, vehicula consectetur massa. Phasellus vulputate metus vitae mi tempus volutpat. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Pellentesque erat metus, lacinia in massa vel, tincidunt blandit turpis. Phasellus ac elit fermentum ipsum pharetra auctor id vel magna. Vivamus a urna venenatis, tempor nisl in, luctus est. Nullam ullamcorper, turpis aliquam accumsan malesuada, purus eros condimentum eros, eget faucibus felis ligula a nunc. Aenean sollicitudin sed nulla et convallis.</p>
            <p>Donec ut ante vel elit accumsan hendrerit nec et urna. Duis et dolor aliquet, sollicitudin ligula eget, iaculis eros. Quisque gravida nisl in sapien suscipit, non efficitur risus pulvinar. Maecenas blandit diam nunc. Sed ac venenatis justo, efficitur accumsan elit. In dictum felis mi, id malesuada ante accumsan at. Vivamus in velit vitae velit placerat gravida ac eget mi.</p>
            <p>Vivamus eget metus eget urna consectetur tristique. Maecenas egestas pretium enim, vel facilisis ante pellentesque ac. Nam maximus sapien nec velit efficitur, at pretium nulla maximus. Phasellus dui quam, commodo vitae pharetra sit amet, viverra a purus. Integer vitae justo sed justo malesuada pretium at sed nibh. Ut semper erat non cursus lobortis. Etiam volutpat magna nec tellus commodo blandit quis ac neque. Duis et dui vitae ex bibendum euismod. Duis ac placerat erat. Curabitur ipsum sem, dapibus nec fringilla quis, mattis sed diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Cras placerat, massa cursus vulputate hendrerit, purus felis dignissim diam, pellentesque ultrices elit ipsum id dolor. Praesent ac sem vitae felis elementum mollis vel ultrices eros. Nullam ut odio lacinia, molestie lorem a, porttitor nibh. Aenean et diam non arcu porta vehicula at ornare libero. Aliquam odio urna, fermentum sed faucibus nec, dapibus in felis.</p>
        </expander>
    </body>
</html>