    #include <Wire.h>
    #include <PN532_I2C.h>
    #include <PN532.h>
    #include <NfcAdapter.h>
    
    PN532_I2C pn532_i2c(Wire);
    NfcAdapter nfc = NfcAdapter(pn532_i2c);
    
    
    

void setup() {
    Serial.begin(115200);
    Serial.println("NDEF Writer");
    nfc.begin();
}

void loop() {
    Serial.println("\nPlace a formatted Mifare Classic NFC tag on the reader.");
    if (nfc.tagPresent()) {
        NdefMessage message = NdefMessage();
        message.addTextRecord("Hello, BikeR!");
        message.addUriRecord("http://bikerweb.azurewebsites.net/");
        message.addTextRecord("bikerId:1234");
        boolean success = nfc.write(message);
        if (success) {
            Serial.println("Success. Try reading this tag with your phone.");
        } else {
            Serial.println("Write failed");
        }
    }
    delay(3000);
}
