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

const greenIcon = new L.Icon({
    iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
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
            addFeatureMarkersToMap(data);
        },

        error: function () {
            alert("Помилка отримання географічної інформації");
        }
    });


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

                L.popup()
                    .setLatLng(e.latlng)
                    .setContent(result.address.Match_addr)
                    .openOn(map);

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


function addFeatureMarkersToMap(geoJSON) {

    const features = JSON.parse(geoJSON);

    L.geoJSON(features, {
        pointToLayer: function (feature, latlng) {

            if (feature.properties.Status === 0) {
                return L.marker(latlng, { icon: redIcon }).bindPopup(getPopupContent(feature));
            } else if (feature.properties.Status === 1) {
                return L.marker(latlng, { icon: greenIcon }).bindPopup(getPopupContent(feature));
            } else {
                return null;
            }
        }
    }).addTo(map);
}


function getPopupContent(feature) {
    const properties = feature.properties;

    let content = '<div>';

    for (let key in properties) {
        if (properties.hasOwnProperty(key)) {
            content += '<b>' + key + ':</b> ' + properties[key] + '<br>';
        }

    }

    content += '<a target="_blank" href="/Complaints/Details/' + properties.Id + '"><span class=>Переглянути скаргу, #' + properties.Id + '</span></a>';

    content += '</div>';

    return content;
}

//TODO hiliteFeatures for filtered Features