enterNVL()
enter('BRAULIO', 'c')

speak('BRAULIO', "Ariel! Hey Ariel!")
speak('ARIEL', "Something the matter?")
speak('BRAULIO', "Not really, but, um. Okay, maybe.")
expr('BRAULIO', 'unsure')
speak('BRAULIO', "Have you seen the news?")
speak('ARIEL', "I try to stay away from TV.")
expr('BRAULIO',  nil)
speak('BRAULIO', "You could also look outside the window. There are students down there.")
speak('ARIEL', "From Aquila?")
speak('BRAULIO', "Yeah. I guess the school paper ran an article about how Lucir set up shop in the tower. And how we're probably going to be funded by the Chey Group.")
speak('ARIEL', "And so they've decided this is the second coming of MKULTRA.")
speak('BRAULIO', "Haha, well um, something like that!")
expr('BRAULIO', 'angry')
speak('BRAULIO', "Turns out if you feed psychedelics to a bunch of lab rats, students don't take it very well.")
expr('BRAULIO', nil)
speak('ARIEL', "You sound dismissive.")
speak('BRAULIO', "Haha, yeah, I know that, but...")
expr('BRAULIO', 'unsure')
speak('BRAULIO', "Ariel, I know I embarassed myself with that stupid story about the goats, and Sumi made fun of me for it, but...")
expr('BRAULIO', 'determined')
speak('BRAULIO', "I really do want Lucir to be something positive in the world.")
expr('BRAULIO', nil)
speak('BRAULIO', "I haven't experienced a tenth of what you and Noemi have gone through. I feel like I'm not qualified to work on a technology like this.")
expr('BRAULIO', 'unsure')
speak('BRAULIO', "Maybe all the Aquila students down there don't really understand what we're up to, but I don't want anyone to hate me. Hate us.")
speak('ARIEL', "Braulio. Take a breath or two.")
speak('BRAULIO', "Ha...")
expr('BRAULIO', 'determined')
speak('BRAULIO', "Yeah, you're right, of course. I'm alright. I'm sorry I freaked out on you.")
speak('ARIEL', "It's fine.")
expr('BRAULIO', nil)
speak('BRAULIO', "You really are a different breed, Ariel. Even Sumi doesn't phase you. Maybe I'm too weak to be a founder.")
speak('ARIEL', "You can't think like that. If you were like Sumi and only interested in the money... Noemi and Chris and I wouldn't be here.")
speak('ARIEL', "You're human. And everyone here values that.")
speak('BRAULIO', "You're way too kind to me Ariel.")
expr('BRAULIO', 'determined')
speak('BRAULIO', "Thanks, though. I'll go brave the protest and head back to my place.")
exitNVL()

wait(0.5)
rotateTo('door')
wait(0.5)

enterNVL()
enter('BRAULIO', 'e')
speak('BRAULIO', "See you tomorrow. And let's get that deal signed!")
exitNVL()

wait(.5)
fade('black')
setSwitch('pt1_10a', true)
fade('normal')
