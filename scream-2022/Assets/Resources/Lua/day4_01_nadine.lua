if getSwitch('day4_01_nadine') then
    speak("Nadine", "Now who's this? Ummm...", 'd4_nadine')
    return
end

speak("Nadine", "Let's see... I guess... Owen.", 'd4_nadine')
wait(1)
speak("Nadine", "Yes! Like I could forget you, you bastard.", 'd4_nadine')
speak("Nadine", "Now who's this?", 'd4_nadine')
speak("Nadine", "Ummm... Allen.", 'd4_nadine')
speak("Nadine", "Knew it! You'll visit me soon, right? Where've you been? Okay next...", 'd4_nadine')
speak("Nadine", "...", 'd4_nadine')
speak("Nadine", "Dang! Cecily, of course. Shoulda known. Okay, next card...", 'd4_nadine')

setSwitch('day4_01_nadine', true)
