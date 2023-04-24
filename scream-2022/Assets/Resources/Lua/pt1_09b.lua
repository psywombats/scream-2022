enterNVL()
enter('chris', 'c')

if not getSwitch('pt1_09b') then
	speak('ARIEL', "Hey Chris. Need a hand?")
	speak('CHRIS', "I'm alright. Just waiting for a titration to finish up before I bring this next batch over to Noemi and the rats.")
	speak('ARIEL', "Any luck on the Bluepill substitutes?")
	speak('CHRIS', "None. But maybe that's a good thing. If Noemi perfects Recurser before I perfect a cyanophazepam analog, that means we don't need any drugs in the first place.")
	speak('CHRIS', "I'm glad you're here though. I did mean to ask you something...")
	expr('CHRIS', 'thinking')
	speak('CHRIS', "Are you sure you want to be allied with Sumi Chey? What do you think of her?")
	speak('ARIEL', "She...")
	speak('ARIEL', "She's smart. I can see why people would think she's charming. But before all else, she's smart. She's not here without a reason.")
	speak('CHRIS', "She's dangerous, is what she is. I left my position at Aquila because I wanted to be known for something other than Bluepill. It's not public that I even work for Lucir.")
	expr('CHRIS', 'pain')
	speak('CHRIS', "So how did she know me? I'm positive I've heard her name before.")
	speak('ARIEL', "You don't think it was a coincidence?")
	expr('CHRIS', nil)
	speak('CHRIS', "No, you're right. I'm being paranoid. I shouldn't be biased against her just because she knew some basic biographical fact about me.")
	speak('CHRIS', "But I don't feel comfortable here with her around. Do we really want to go public with a version of Recurser that still requires the patient to take Bluepill?")
	speak('ARIEL', "That's why we're working on a workaround.")
	speak('CHRIS', "But if we don't find that workaround before the next version of Recurser is complete... You think a venture capitalist would care?")
	expr('CHRIS', 'pain')
	speak('CHRIS', "We'd go to market with what worked, ethics be damned.")
	expr('CHRIS', nil)
	speak('ARIEL', "Is Bluepill itself really unethical, though? It's been proven to be nonaddictive.")
	speak('CHRIS', "But consider Noemi. Or any of those cases from China or Indonesia or Russia about people dreamwalking into traffic or off balconies.")
	speak('CHRIS', "It might not be addictive, but...")
	speak('CHRIS', "For some, the temptation to escape is just too high.")
	expr('CHRIS', 'thinking')
	speak('CHRIS', "If we ship with Bluepill still in the formula...")
	expr('CHRIS', nil)
	speak('CHRIS', "Sorry, Ariel, I'll probably quit.")
	speak('ARIEL', "I'd hate to see you go.")
	speak('CHRIS', "And I'd hate to leave you and Noemi here, but...")
	expr('CHRIS', 'pain')
	speak('CHRIS', "Ahh, just ignore me. I shouldn't be saying any of this to anyone.")
	expr('CHRIS', nil)
	speak('ARIEL', "It's alright. I'm glad you trust me.")
	speak('CHRIS', "Go on back to being happy with Braulio that we're finally funded. I'll finish up my work here.")
else
	speak('CHRIS', "Go on back to being happy with Braulio that we're finally funded. I'll finish up my work here.")
end

exitNVL()
setSwitch('pt1_09b', true)
