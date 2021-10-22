if getSwitch('day2_04_owen') then
    speak("Owen", "Well? Any luck?")
    return
end

speak("Owen", "You seem out of sorts.", 'd2_owen2')
speak("Tess", "I am.")
speak("Tess", "You honestly don't remember looking for Connie before?")
speak("Owen", "No. But obviously, you know I'm N9 positive. Forgetting random events is hardly unusual.", 'd2_owen2')
speak("Tess", "This just happened yesterday.")
speak("Owen", "If that were true, everyone else would remember as well. No way we all forgot something that recent.", 'd2_owen2')
speak("Tess", "Then what do you think is happening?")
speak("Owen", "Deja vu.", 'd2_owen2')
speak("Tess", "This is more than that.")
speak("Owen", "It's possible that N9 could cause phenomena like false memories and deja vu to become more vivid...", 'd2_owen2')
speak("Tess", "Maybe you'll believe me if I remember where Connie was hiding.")

setSwitch('day2_04_owen', true)
