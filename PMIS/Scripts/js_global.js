class utilities {
    
    static toProperCase(data) {

        //debugger;
        var strdata = data;

        return strdata.replace(/\w\S*/g,
            function (txt) {
                return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
            });
    }

}