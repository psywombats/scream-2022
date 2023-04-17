pathTo('d1_target1')

speak("Tess", "What's going on?")
faceTo('d1_owen1')
speak("Owen", "Joey's throwing a fit.", 'd1_owen1')
faceTo('d1_nadine1')
speak("Nadine", "Hey, calm down, calm down. Blow your nose.", 'd1_nadine1')
speak("Joey", "Someone must've left the hatch open! She could be anywhere!", 'd1_joey1')
walk('d1_lia1', 2, 'EAST')
face('d1_lia1', 'SOUTH')
speak("Lia", "Tess, it's the rabbit. I think she got loose.", 'd1_lia1')
speak("Owen", "That's the gist of it. Probably the thing made it to another floor.", 'd1_owen1')
speak("Owen", "...and it's not like we're allowed off this one.", 'd1_owen1')
face("d1_joey1", 'SOUTH')
face("d1_nadine1", 'SOUTH')
speak("Joey", "What do we do?", 'd1_joey1')
speak("Tess", "We'll look for Connie in Ward #6. Dr. Cooper can check the other floors.")
speak("Tess", "If we all look, I'm sure we can find her.")
speak("Joey", "Okay. *sniff*", 'd1_joey1')
speak("Joey", "Thanks Tess. I don't know what I'd do without Connie.", 'd1_joey1')
speak("Nadine", "I'll start here in the common room. Who's gonna talk to the docs?", 'd1_nadine1')
speak("Tess", "I'll do it.")
speak("Owen", "That's probably a good idea.", 'd1_owen1')
speak("Owen", "Everyone else, start with your own room, and then we can cover the other spots.", 'd1_owen1')
speak("Lia", "Tess, um... Can I come with you?", 'd1_lia1')
speak("Tess", "That's fine.")
speak("Lia", "I just don't want to get lost.", 'd1_lia1')

fade('black')
spawnFollower('lia_bot', 'liabot_target')
setSwitch('spawn_lia', true)
setSwitch('day1_08_meeting', true)
fade('normal')