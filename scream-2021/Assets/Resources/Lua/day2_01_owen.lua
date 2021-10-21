if getSwitch('day2_01_owen') then
    speak("Tess", "I've got my checkup with Dr. Cooper. Talk later.")
    return
end

speak("Owen", "Any luck last night?", 'd2_owen0')
speak("Tess", "The stairs don't really go anywhere. They lead to the research lab attached to the ward, not the outside world.")
speak("Owen", "Interesting.", 'd2_owen0')
speak("Owen", "How much do you know about Allsaints Hospital?", 'd2_owen0')
speak("Tess", "It's a pediatric hospital, for kids and young adults. It has twelve departments and was founded in - ")
speak("Owen", "No, never mind, stop typing. You're just copying verbatim what you've read off their pamphlets.", 'd2_owen0')
speak("Owen", "What if this wasn't a hospital, but just one ward? Ward #6 and a lab. Why else would you have a staircase like that?", 'd2_owen0')
speak("Tess", "Maybe they expect you to use the elevator.")
speak("Owen", "Ugh. Never mind. But the point is, it's a dead end, and not a way out...", 'd2_owen0')
speak("Tess", "Not necessarily. I didn't get to finish exploring.")
speak("Owen", "It's that big?", 'd2_owen0')
speak("Tess", "No.")
wait(.3)
speak("Tess", "I got caught.")
speak("Owen", "You what? Ugh, of all the - ", 'd2_owen0')

setSwitch('day2_01_owen', true)
face('hero', 'WEST')
face('d2_owen0', 'WEST')
walk('d2_nadine0', 6, 'EAST')

speak("Nadine", "Hey, what're you guys up to?", 'd2_nadine0')
speak("Owen", "Nothing.", 'd2_owen0')
speak("Nadine", "You're such a bad liar Owen. You're over here whispering to Tess about something, hmmm?", 'd2_nadine0')
speak("Owen", "Never mind.", 'd2_owen0')
speak("Tess", "I've got my checkup with Dr. Cooper now anyway. Talk later.")
