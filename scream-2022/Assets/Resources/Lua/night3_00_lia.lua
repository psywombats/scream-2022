if getSwitch('night3_00_lia') then
    speak("Tess", "Goodnight, Lia.")
    return
end

speak("Lia", "Tess? Where are you going?", 'n3_lia')
speak("Tess", "Don't worry about it.")
speak("Lia", "D-did know there's a curfew?", 'n3_lia')
speak("Tess", "Yes. Don't worry about it.")
speak("Lia", "Maybe I should come with you.", 'n3_lia')
wait(.8)
speak("Tess", "I really think Owen might've been right about you.")
speak("Tess", "Are you really just a patient here?")
speak("Lia", "...", 'n3_lia')
speak("Tess", "Goodnight, Lia.")

setSwitch('night3_00_lia', true)
