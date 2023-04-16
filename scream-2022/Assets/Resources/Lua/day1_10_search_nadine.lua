if getSwitch('day1_10_search_nadine') then
    speak("Nadine", "Joey's gonna be devastated if we don't get Connie back.", 'd1_nadine1')
    return
end

faceOther('lia_bot', 'd1_nadine1')
speak("Nadine", "Hey Tess! And ummm...", 'd1_nadine1')
faceOther('d1_nadine1', 'lia_bot')
speak("Lia", "It's - ", 'lia_bot')
speak("Nadine", "Lia! I'm right, aren't I?", 'd1_nadine1')
speak("Lia", "Yeah. That's it.", 'lia_bot')
speak("Tess", "Good job, Nadine.")
speak("Nadine", "Haha, don't say it's a good job until I've found that little bunny.", 'd1_nadine1')
speak("Nadine", "Joey's gonna be devastated if we don't get Connie back.", 'd1_nadine1')
speak("Lia", "We'll find her.", 'lia_bot')
speak("Nadine", "I like you. Just showed up today and already pitching in!", 'd1_nadine1')

setSwitch('day1_10_search_nadine', true)
