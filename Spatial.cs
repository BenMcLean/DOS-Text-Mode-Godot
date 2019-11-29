using Godot;
using System;

public class Spatial : Godot.Spatial
{
	private DosScreen DosScreen;
	private DateTime now = DateTime.Now;
	private Random random = new Random();

	public override void _Ready()
	{
		VisualServer.SetDefaultClearColor(Color.Color8(0, 0, 0, 255));

		Camera camera = new Camera()
		{
			Current = true,
		};
		camera.SetScript(ResourceLoader.Load("res://maujoe.camera_control/camera_control.gd") as GDScript);
		AddChild(camera);

		AddChild(new WorldEnvironment()
		{
			Environment = new Godot.Environment()
			{
				BackgroundColor = Color.Color8(85, 85, 85, 255),
				BackgroundMode = Godot.Environment.BGMode.Color,
			},
		});

		AddChild(DosScreen = new DosScreen()
		{
			GlobalTransform = new Transform(Basis.Identity, new Vector3(0, 0, -2)),
		});
	}

	public override void _Process(float delta)
	{
		if (DateTime.Now - now > TimeSpan.FromSeconds(1))
		{
			now = DateTime.Now;
			double trouble = random.NextDouble();
			string message;
			if (trouble < 1d / 3d)
				message = "Short message ";
			else if (trouble < 2d / 3d)
				message = "Slightly longer message ";
			else //if (trouble < 0.9d)
				message = "Really really long but not quite the longest message ";
			//else
			//	message = "The Boy Bands Have Won, and All the Copyists and the Tribute Bands and the TV Talent Show Producers Have Won, If We Allow Our Culture to Be Shaped by Mimicry, Whether from Lack of Ideas or from Exaggerated Respect. You Should Never Try to Freeze Culture. What You Can Do Is Recycle That Culture. Take Your Older Brother's Hand-Me-Down Jacket and Re-Style It, Re-Fashion It to the Point Where It Becomes Your Own. But Don't Just Regurgitate Creative History, or Hold Art and Music and Literature as Fixed, Untouchable and Kept Under Glass. The People Who Try to 'Guard' Any Particular Form of Music Are, Like the Copyists and Manufactured Bands, Doing It the Worst Disservice, \nBecause the Only Thing That You Can Do to Music That Will Damage It Is Not Change It, Not Make It Your Own. Because Then It Dies, Then It's Over, Then It's Done, and the Boy Bands Have Won ";

			DosScreen.Screen.WriteLine(message + now.ToString());
		}
	}
}
