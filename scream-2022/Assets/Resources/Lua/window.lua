if not getSwitch('pt1_08') then
	speak('ARIEL', "The midday sky. From the 37th floor, you can see all of Aquila University, and most of the city too.")
elseif not getSwitch('pt1_10') then
	speak('ARIEL', "It's late afternoon. Looks like the sun is starting to go down.")
elseif not getSwitch('midnight') then
	speak('ARIEL', "Night in the city. Thousands and thousands of streetlights and headlamps are refracted in the fog.")
elseif not getSwitch('pt1_done') then
	speak('ARIEL', "The witching hour. I am lucid. Reality is mine to command.")
elseif not getSwitch('pt2_08') then
	speak('ARIEL', "It's a fantastic day out there. I just wish I remembered more of it.")
elseif not getSwitch('finale_mode') then
	speak('ARIEL', "The sun set hours ago. If I'm not careful, I could run out of time too.")
else 
	speak('ARIEL', "The truth is mine. I will not falter.")
end
