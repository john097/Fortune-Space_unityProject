var splash: Transform;

function LateUpdate () 
{
	var theParticles = GetComponent.<ParticleEmitter>().particles;
	
	for(var i = 0; i < GetComponent.<ParticleEmitter>().particleCount;i++)
	{
		if(theParticles[i].energy > GetComponent.<ParticleEmitter>().maxEnergy)
		{
			var splashObj: Transform = Transform.Instantiate(splash, theParticles[i].position, Quaternion.identity);
			theParticles[i].energy = 0;
			Destroy(splashObj.gameObject, 0.5);
		}
	}	
	GetComponent.<ParticleEmitter>().particles = theParticles;
}