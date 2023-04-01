var map;
var popup;
var highlightLayer;

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

    popup = L.popup();

    function onMapClick(e) {
        popup
            .setLatLng(e.latlng)
            .setContent("Координати точки: " + e.latlng.toString())
            .openOn(map);
    }

    map.on('click', onMapClick);

});


function featuresToMap(complaintFeaturesJSON) {

    var parsedFeatures = JSON.parse(complaintFeaturesJSON);

    L.geoJSON(parsedFeatures).addTo(map);

}

//TODO hiliteFeatures for filtered Features