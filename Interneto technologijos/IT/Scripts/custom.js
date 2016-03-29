var uri = 'api/Default';

$(document).ready(function () {
    // Send an AJAX request
    $.getJSON(uri)
        .done(function (data) {
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $('#allFlights > tbody:last-child').append(formatItem(item));
            });
        });

    $('#dateTill').val("2015-12-31");

    $('#search').on('submit', find);
    
    // actualy does not filter anything, just a simulation
    $('#filter').on('submit', function () {
        // 2 JS reikalavimas
        var daysTillArrival = Number(document.getElementById('numberOfDays').value);

        if (daysTillArrival < 0 || !isInt(daysTillArrival)) {
            alert("Dienos turi būti sveikas skaičius nuo 0");
            return false;
        }

        // 3 JS reikalavimas
        var datePattern = /(\d{4})-(\d{2})-(\d{2})/;
        var dateTillString = document.getElementById('dateTill').value;


        if (datePattern.test(dateTillString)) {
            // matches format
            // 3 regexp reikalavimas
            var day = dateTillString.replace(datePattern, "$3");
            var month = dateTillString.replace(datePattern, "$2");
            var year = dateTillString.replace(datePattern, "$1");

            var datetill = new Date(year, month-1, day);

            /*
            console.log(datetill.getDate());
            console.log(day);
            console.log(datetill.getMonth());
            console.log(month);
            */

            if (datetill.getDate() == day && month > 0 && month < 13) {
                console.log("good date");
                $('#allFlightsTableBody').hide('slow');
                $('#allFlightsTableBody').show('slow');

                // jquery reikalavimas 1,2 
                var headerTitle = $('header p');
                headerTitle.text($('header p').text() + " (nufiltruotas)");
                headerTitle.attr("style", "color: darkblue;");

                return false;
            } else {
                alert("Neteisinga data.");

                // jquery 3 reikalavimas
                $('#allFlights > tbody > tr').remove();
                return false;
            }
        } else {
            alert("Neteisinga data. Formatas turi buti yyyy-MM-dd");
            return false;
        }


    });
});

function isInt(n) {
    return Number(n) === n && n % 1 === 0;
}

function formatItem(item) {
    var nowDate = new Date();

    return $(document.createElement("tr")).html(
        "<td>" + item.FlightCode + "</td>"
        + "<td>" + item.FromAirportCode + "</td>"
        + "<td>" + item.ToAirportCode + "</td>"
        + "<td>" + formatDateTime(new Date(item.Departure)) + "</td>"
        + "<td " + ((new Date(item.Arrival)) <= nowDate.setDate(nowDate.getDate() + 7) ? "class=\"soon\"" : "") + ">" + formatDateTime(new Date(item.Arrival)) + "</td>"
        + "<td>" + item.Cost + "</td>");

}

function formatDateTime(item) {
    return (item.getFullYear() + "-" + (item.getMonth() + 1) + "-" + item.getDate()
        + "  " + item.getHours() + ":" + item.getMinutes() + ":" + item.getSeconds()).toString();
}

function find() {
    var code = $('#flightCode').val();
    if (!code) { code = "all" };
    var fromAirportCode = $('#airportFromCode').val();
    if (!fromAirportCode) { fromAirportCode = "all" };
    var toAirportCode = $('#airportToCode').val();

    // 1 JS reikalavimas
    if (!toAirportCode) {
        alert("būtina įvesti oro uosto \"į\" kodą");
        return false;
    };

    $.getJSON(uri + '/' + code + '/' + fromAirportCode + '/' + toAirportCode)
        .done(function (data) {
            $('#SearchTable > tbody').empty();
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $('#SearchTable > tbody:last-child').append(formatItem(item));
            });
        })
        .fail(function (jqXHR, textStatus, err) {
            alert('Error: ' + err);
        });

    return false;
}

// regexp reikalavimai
function regexpTest() {
    var string = "        as 1994 gimiau o dabar, 2015 labai daug valgau, rytoj dar daugiau valgysiu";

    // gauna pirma zodi
    var regExp1 = /^\s*(\w+)\s+/;
    var match = regExp1.exec(string);
    console.log("pirmas zodis - " + match[1]);

    // visi zodziai kurie baigiai su balse 
    var regExp2 = /\s*(\w*[aeiou])\W+/g;
    console.log("zodziai, kurie baigiasi balse: ");
    while((match = regExp2.exec(string)) != null)
    {
        console.log("  " + match[1]);
    }

    // pirmas skaicius
    var regExp3 = /\d+?/;
    var match3 = regExp3.exec(string);
    console.log("pirmas skaicius " + match3);

    // sakinio dalys
    var regExp4 = /\w,/;
    var splitString = string.split(regExp4);
    console.log("sakinio pirma dalis " + splitString[0]);

 



}