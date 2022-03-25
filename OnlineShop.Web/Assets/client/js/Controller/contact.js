
// This example displays a marker at the center of Australia.
// When the user clicks the marker, an info window opens.
function initMap() {
    const uluru = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 17,
        center: uluru,
    });
    const contentString =
        '<div id="content">' +
        '<div id="siteNotice">' +
        "</div>" +
        '<h1 id="firstHeading" class="firstHeading">Uluru</h1>' +
        '<div id="bodyContent">' +
        "<p><b>Uluru</b>, also referred to as <b>Ayers Rock</b>, is a large " +
        "sandstone rock formation in the southern part of the " +
        "Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) " +
        "south west of the nearest large town, Alice Springs; 450&#160;km " +
        "(280&#160;mi) by road. Kata Tjuta and Uluru are the two major " +
        "features of the Uluru - Kata Tjuta National Park. Uluru is " +
        "sacred to the Pitjantjatjara and Yankunytjatjara, the " +
        "Aboriginal people of the area. It has many springs, waterholes, " +
        "rock caves and ancient paintings. Uluru is listed as a World " +
        "Heritage Site.</p>" +
        '<p>Attribution: Uluru, <a href="https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194">' +
        "https://en.wikipedia.org/w/index.php?title=Uluru</a> " +
        "(last visited June 22, 2009).</p>" +
        "</div>" +
        "</div>";
    const infowindow = new google.maps.InfoWindow({
        content: contentString,
    });
    const marker = new google.maps.Marker({
        position: uluru,
        map,
        title: $('#hidName').val()
    });
    marker.addListener("click", () => {
        infowindow.open({
            anchor: marker,
            map,
            shouldFocus: false,
        });
    });
}

initMap();



//var contact = {
//    init: function () {
//        contact.registerEvent();
//    },
//    registerEvent: function () {
//        contact.initMap();
//    },
//    initMap: function () {
//        var uluru = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
//        var map = new google.maps.Map(document.getElementById('map'), {
//            zoom: 17,
//            center: uluru
//        });

//        var contentString = $('#hidAddress').val();

//        var infowindow = new google.maps.InfoWindow({
//            content: contentString
//        });

//        var marker = new google.maps.Marker({
//            position: uluru,
//            map: map,
//            title: $('#hidName').val()
//        });

//        marker.addListener("click", () => {
//            infowindow.open({
//                anchor: marker,
//                map,
//                shouldFocus: false,
//            });
//        });

//       infowindow.open(map, marker);
//    }
//}

//contact.init();



//function initMap(): void {
//    const uluru = { lat: -25.363, lng: 131.044 };
//    const map = new google.maps.Map(
//        document.getElementById("map") as HTMLElement,
//        {
//            zoom: 4,
//            center: uluru,
//        }
//    );

//    const contentString =
//        '<div id="content">' +
//        '<div id="siteNotice">' +
//        "</div>" +
//        '<h1 id="firstHeading" class="firstHeading">Uluru</h1>' +
//        '<div id="bodyContent">' +
//        "<p><b>Uluru</b>, also referred to as <b>Ayers Rock</b>, is a large " +
//        "sandstone rock formation in the southern part of the " +
//        "Northern Territory, central Australia. It lies 335&#160;km (208&#160;mi) " +
//        "south west of the nearest large town, Alice Springs; 450&#160;km " +
//        "(280&#160;mi) by road. Kata Tjuta and Uluru are the two major " +
//        "features of the Uluru - Kata Tjuta National Park. Uluru is " +
//        "sacred to the Pitjantjatjara and Yankunytjatjara, the " +
//        "Aboriginal people of the area. It has many springs, waterholes, " +
//        "rock caves and ancient paintings. Uluru is listed as a World " +
//        "Heritage Site.</p>" +
//        '<p>Attribution: Uluru, <a href="https://en.wikipedia.org/w/index.php?title=Uluru&oldid=297882194">' +
//        "https://en.wikipedia.org/w/index.php?title=Uluru</a> " +
//        "(last visited June 22, 2009).</p>" +
//        "</div>" +
//        "</div>";

//    const infowindow = new google.maps.InfoWindow({
//        content: contentString,
//    });

//    const marker = new google.maps.Marker({
//        position: uluru,
//        map,
//        title: "Uluru (Ayers Rock)",
//    });

//    marker.addListener("click", () => {
//        infowindow.open({
//            anchor: marker,
//            map,
//            shouldFocus: false,
//        });
//    });
//}




















