speak("Dr. Cooper", "Hey Tess! Who's this fresh face?", 'd1_cal0')
speak("Tess", "Dr. Cooper, this is Lia. The new patient.")
speak("Lia", "N-nice to meet you Dr. Cooper.", 'lia_bot')
speak("Dr. Cooper", "Just Cal is fine. And that goes for you too, Tess, for the thousandth time.", 'd1_cal0')
speak("Tess", "I'll keep that in mind, Dr. Cooper.")
speak("Dr. Cooper", "Sheesh...", 'd1_cal0')
speak("Dr. Cooper", "Anyways Lia, I'm one of the doctors assigned to this ward.", 'd1_cal0')
speak("Dr. Cooper", "Well, more like a researcher than a doctor.", 'd1_cal0')
speak("Dr. Cooper", "But I'm sure we'll be getting to know each other soon!", 'd1_cal0')
speak("Dr. Cooper", "I'm expecting some result in so I gotta run, but, see you at 4:00, right Tess?", 'd1_cal0')
speak("Tess", "Right.")
speak("Dr. Cooper", "Seeya!", 'd1_cal0')
walk('d1_cal0', 7, 'EAST')

setSwitch('day1_03_intro_cal', 'true')