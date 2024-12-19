# PS5CodeReader
This is the "living" branch of amoamare's original PS5 Code Reader project. 
Check out the latest revision 1.2.0.2 that now implements decoding of more detailed error information. 

To my knowledge, the full error record contains the following information:

  __OK 00000000 [ERROR_CODE] [RTC_DATE_TIME] [PWR STATE] [UP CAUSE] [SEQ NUM] [DEV PM INFO] [SOC TEMP] [ENV TEMP]__

- RTC_DATE_TIME -> This is the timestamp of the event. Ps5 code reader displayed timestamp as a time difference, compared to the last/latest error. I have not found a way to get the base offset the clock is ticking against, so I just display differences which I have found pretty handy.
- PWR_STATE -> system power state information
- UP_CAUSE -> Gives a hint on what method was used to power up the unit (like power button, feeding a disk, HDMI CEC, ...)
- SEQ_NUM -> Some kind of sequence number related to boot up process. Maybe this too gives a hint on what the system was doing (which init phase) when the error triggered. Let me know if any known mapping to something useful is found.
- DEV_PM_INFO -> Power Managemenet information. Shown with letters "HBCUW" that stand for:
  - H --> HDMI
  - B --> Blueray Drive
  - C --> HDMI CEC 
  - U --> USB VBUS Output
  - W --> WiFi Power
- SOC_TEMP -> This is the temperature measurement from the main System on Chip (SoC, a.k.a APU)
- ENV_TEMP -> This is a temperature reading from an "environment" temperature sensor (I guess the one on LED PCA.)

Screenshots:

![read_codes_detail](https://github.com/user-attachments/assets/f569322a-35a9-40f8-89aa-be7b60fd0404)
![read_codes](https://github.com/user-attachments/assets/7a18f02d-379d-4cbf-a2ab-353c9b633df2)
![RawCmd](https://github.com/RustyRaindeer/PS5CodeReader/assets/6859581/1c7897d6-8c16-4827-90e1-f740d0246629)
![image](https://github.com/amoamare/PS5CodeReader/assets/15149902/dced1bb1-2632-40c6-af06-8f851e5eeb78)
