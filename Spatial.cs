using Godot;
using System;

public class Spatial : Godot.Spatial
{
	private VirtualScreenText virtualScreenText = new VirtualScreenText();

	private DateTime now = DateTime.Now;
	private Random random = new Random();

	public override void _Ready()
	{
		VisualServer.SetDefaultClearColor(Color.Color8(0, 0, 0, 255));

		Camera camera = new Camera()
		{
			Current = true,
		};
		AddChild(camera);

		WorldEnvironment worldEnvironment = new WorldEnvironment()
		{
			Environment = new Godot.Environment()
			{
				BackgroundColor = Color.Color8(85, 85, 85, 255),
				BackgroundMode = Godot.Environment.BGMode.Color,
			},
		};
		AddChild(worldEnvironment);

		Viewport viewport = new Viewport()
		{
			Size = new Vector2(720, 400),
			Disable3d = true,
			RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame,
			RenderTargetVFlip = true,
		};
		AddChild(viewport);
		viewport.AddChild(new ColorRect()
		{
			Color = Color.Color8(0, 0, 0, 255),
			RectSize = viewport.Size,
		});

		AddChild(new Sprite3D()
		{
			Texture = viewport.GetTexture(),
			PixelSize = 0.00338666666f,
			Scale = new Vector3(1f, 1.35f, 1f),
			MaterialOverride = new SpatialMaterial()
			{
				FlagsUnshaded = true,
				FlagsDoNotReceiveShadows = true,
				FlagsDisableAmbientLight = true,
				ParamsSpecularMode = SpatialMaterial.SpecularMode.Disabled,
				ParamsCullMode = SpatialMaterial.CullMode.Disabled,
				FlagsTransparent = false,
			},
			GlobalTransform = new Transform(Basis.Identity, new Vector3(0, 0, -2)),
		});

		BitmapFont font = new BitmapFont();
		font.CreateFromFnt("Bm437_IBM_VGA9.fnt");
		Theme theme = new Theme()
		{
			DefaultFont = font,
		};

		Label label = new Label()
		{
			Theme = theme,
			RectPosition = new Vector2(0, 0),
		};
		label.Set("custom_constants/line_spacing", 0);
		label.Set("custom_colors/font_color", Color.Color8(170, 170, 170, 255));
		virtualScreenText.Label = label;
		viewport.AddChild(label);
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
			else if (trouble < 0.9d)
				message = "Really really long but not quite the longest message ";
			else
				message = "The Boy Bands Have Won, and All the Copyists and the Tribute Bands and the TV Talent Show Producers Have Won, If We Allow Our Culture to Be Shaped by Mimicry, Whether from Lack of Ideas or from Exaggerated Respect. You Should Never Try to Freeze Culture. What You Can Do Is Recycle That Culture. Take Your Older Brother's Hand-Me-Down Jacket and Re-Style It, Re-Fashion It to the Point Where It Becomes Your Own. But Don't Just Regurgitate Creative History, or Hold Art and Music and Literature as Fixed, Untouchable and Kept Under Glass. The People Who Try to 'Guard' Any Particular Form of Music Are, Like the Copyists and Manufactured Bands, Doing It the Worst Disservice, \nBecause the Only Thing That You Can Do to Music That Will Damage It Is Not Change It, Not Make It Your Own. Because Then It Dies, Then It's Over, Then It's Done, and the Boy Bands Have Won ";

			virtualScreenText.WriteLine(message + now.ToString());
		}
	}
}
