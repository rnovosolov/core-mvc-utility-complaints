var map;
var popup;

const apiKey = "AAPKec9a5e24f87b46c09b9de4d32c0161ackmUl8iO033ZjJCgehLOQwp0DjOU5WakwmSAJu9u6BTKv9mVJyuMGFVDEydgRdVGe";

const redIcon = new L.Icon({
    iconUrl:
        "https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-red.png",
    shadowUrl:
        "https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
});

//TODO: data fit in map 
var data_bounds;
var map_bounds;

$(document).ready(function () {

    map = new L.map("map", {
        ctrlClick: true,
        wheelPxPerZoomLevel: 120,
        zoomDelta: 0.5,
        zoomSnap: 0.5,
        center: [50.45008, 30.523446],
        minZoom: 6,
        zoom: 11,
        maxZoom: 18

    });

    L.control.scale({ metric: true, imperial: false, maxWidth: 200 }).addTo(map);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);


    $.ajax({
        type: "GET",
        url: '/Complaints/GetFeatures',
        data: '[]',
        dataType: "html",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            featuresToMap(data);
        },

        error: function () {
            alert("Помилка отримання географічної інформації");
        }
    });

    /*popup = L.popup();

    function onMapClick(e) {
        popup
            .setLatLng(e.latlng)
            .setContent("Координати точки: " + e.latlng.toString())
            .openOn(map);
    }

    map.on('click', onMapClick);*/

    map.on("click", function (e) {
        L.esri.Geocoding 
            .reverseGeocode({
                apikey: apiKey
            })
            .latlng(e.latlng)
            .run(function (error, result) {
                if (error) {
                    return;
                }

                L.marker(result.latlng) //delete previous marker
                    .addTo(map).
                    bindPopup(result.address.Match_addr + '<br>' + e.latlng.toString())
                    .openPopup();//.openOn(map);


                var popupText = result.address.Match_addr;
                document.getElementById("addr").value = popupText;
                var popupLat = e.latlng.lat.toFixed(6).toString();
                document.getElementById("lat").value = popupLat;
                var popupLon = e.latlng.lng.toFixed(6).toString();
                document.getElementById("lon").value = popupLon;
                //parse district from address
            });
    });

});


function featuresToMap(complaintFeaturesJSON) {

    var parsedFeatures = JSON.parse(complaintFeaturesJSON);

    L.geoJSON(parsedFeatures, {
        onEachFeature: function (feature, layer) {

            var popupContent = '';
            var prop = feature.properties;
            Object.keys(prop).forEach(function (key, index) {
                popupContent += '<b>' + key + '</b>:' + prop[key] + '<br>';
            });
            popupContent += '<a target="_blank" href="/Complaints/Details/' + prop.Id + '"><span class=>Переглянути скаргу, #' + prop.Id + '</span></a>';
            layer.bindPopup(popupContent);

            /*layer.bindPopup('<b>Адреса: </b>' + feature.properties.Address + '<br>' 
                + '<b>Опис: </b>' + feature.properties.Description + '</p>');*/

        }
    }).addTo(map);

}


//TODO hiliteFeatures for filtered Features