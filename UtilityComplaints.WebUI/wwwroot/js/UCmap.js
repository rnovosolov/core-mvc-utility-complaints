

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
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            //draw complaintFeaturesJSON on map
            loadAssetGeojson(data);
        },

        error: function () {
            alert("Помилка отримання географічної інформації");
        }
    });
});






var popup = L.popup();

function onMapClick(e) {
    popup
        .setLatLng(e.latlng)
        .setContent("Координати точки: " + e.latlng.toString())
        .openOn(map);
}

map.on('click', onMapClick);



function loadAssetGeojson(complaintFeaturesJSON) {

    //var complaintFeaturesParsed = JSON.parse(complaintFeaturesJSON); //not parsing
    alert("Помилка обробки географічної інформації");
    asset = L.geoJson(complaintFeaturesJSON, {
        pointToLayer: function (feature, latlng) {
            var marker = L.marker(latlng, { icon: makeAssetIconHtml(feature, true) });
            //not returning
            return marker;
        },

        onEachFeature: function (feature, layer) {

            assets[parseInt(feature.properties.Id)] = layer;

            var str = '';
            var obj = feature.properties;
            Object.keys(obj).forEach(function (key, index) {
                str += '<b>' + key + '</b>:' + obj[key] + '<br>';
            });
            str += '<a target="_blank" href="/Complaints/Details/' + obj.Id + '"> <span class=>переглянути Скаргу #' + obj.Id + '</span></a>';
            layer.bindPopup(str);

        }

    });
    if (typeof clusterGroup != 'undefined') map.removeLayer(clusterGroup);
    clusterGroup.addLayer(asset);
    map.addLayer(clusterGroup)

    $(document).trigger("featuresloaded");
    //lgroup = L.featureGroup(clusterGroup).addTo(map);

    //map.fitBounds(lgroup.getBounds());
    //map.fitBounds(asset.getBounds());

}

//TODO hiliteFeatures for filtered Features