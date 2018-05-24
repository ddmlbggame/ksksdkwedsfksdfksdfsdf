//#define MANUAL_UNLOAD
//#define AUTO_UNLOAD
//#define SHOW_DETAIL_LOG
#define COMMON_DONT_UNLOAD

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Assertions;
using Entity = UnityEngine.GameObject;
using UObject = UnityEngine.Object;
using UnityEngine.UI;


namespace Paran
{
	// AssetManager의 참고용 뼈대 코드 이다.
	// 이 코드는 테스트를 거치지 않았으며, 뼈대만 보여주기 위한 목적이다.
	// 또한 UNITY_EDITOR에서 동작을 어떻게 해야 하는지에 대한 코드도 포함하지 않고 있다.
	// 또한 loading 중 발생한 다양한 error 및 예외 사항도 처리하지 않았다.
	// 또한 의존 asset bundle에 대한 처리도 고려되지 않았다.
	// 여기서 고려하지 않은 모든 사항은 추가 코딩을 하고, 테스트를 반드시 거쳐서 정상실행을 확인해야 한다.

	public static class AssetManager
	{
		#region Manifest

#if UI_RESOURCE
		public static class Manifest
		{

			static public IEnumerator Init()
			{
				yield break;
			}

			static public IEnumerator Patch()
			{
				yield break;
			}

			public static bool DependenciesDoneRecursive( Bundle base_bundle )
			{
				foreach( var item in base_bundle.Dependencies )
				{
					var www = item.BundleWww;
					if( www == null )
					{
						continue;
					}
					if( !www.isDone )
						return false;

					item.BundleObject = www.assetBundle;

					// 					if (!DependenciesDone(item))
					// 						return false;
				}
				return true;
			}

			public static Hash128 GetHash( string asset_bundle_name )
			{
				return Bundle.Empty;
			}

			// Dependencies & Load ABLE Check 
			public static bool DependenciesDone( Bundle base_bundle )
			{
				return false;
			}

#if UNITY_EDITOR

			static public T LoadAssetDataBase<T>( string bundle_name, string asset_name ) where T : UObject
			{
				T value = null;
				string strAssetName = System.IO.Path.GetFileNameWithoutExtension( asset_name );
				var assetDependencies = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( bundle_name );
				foreach( var item in assetDependencies )
				{
					string strItemName = System.IO.Path.GetFileNameWithoutExtension( item );
					if( strItemName == strAssetName )
					{
						value = UnityEditor.AssetDatabase.LoadAssetAtPath<T>( item );
						break;
					}
				}
				return value;
			}

			static public T LoadAssetDataBase<T>( string bundle_name, string asset_name, int multple_index ) where T : UObject
			{
				T value = null;
				var assetDependencies = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( bundle_name );
				foreach( var item in assetDependencies )
				{
					if( item.Contains( asset_name ) )
					{
						if( multple_index <= -1 )
						{
							value = UnityEditor.AssetDatabase.LoadAssetAtPath<T>( item );
						}
						else
						{
							var arrValue = UnityEditor.AssetDatabase.LoadAllAssetsAtPath( item );
							for( int i = 0; i < arrValue.Length; ++i )
							{
								if( arrValue[i].name.EndsWith( "_" + ( multple_index ) ) )
								{
									value = (T)arrValue[i];
								}
							}
						}

						break;
					}
				}
				return value;
			}
#endif
		}
#else

        public static class Manifest
		{
			static Bundle _manifest_bundle;
			static AssetBundleManifest _manifest;
			static string _file_name;

			static public IEnumerator Init()
			{
				//get AssetBundleManifest
				if( null == _manifest_bundle
#if UNITY_EDITOR
				&& UseBundleInEditor
#endif
				)
				{
					var paths = BundlePath.Split( '/' );
					_file_name = string.Format( "{0}", paths[paths.Length - 2] );

#if !UNITY_EDITOR
                    if (_file_name == "assets")//for test  empty out path 
                        _file_name = "StreamingAssets";
                    //if (string.Equals(BundlePath, Application.streamingAssetsPath + "/"))//for test  empty out path 
                    //    _file_name = "StreamingAssets"; 
#endif

					if( 0 < paths.Length )
					{
						_manifest_bundle = _LoadBundleAsyncWww( WwwBundlePath + _file_name, _file_name );
						//_manifest_bundle = LoadBundle(WwwBundlePath + _file_name);                        
					}
				}

				if( _manifest_bundle != null )
				{
					var www = _manifest_bundle.BundleWww;


					if( null != www.error )
						Develop.Log( www.error );

					while( !www.isDone )
					{
						yield return www;
					}
					if( null != www.assetBundle )
					{
						_manifest = www.assetBundle.LoadAsset( "AssetBundleManifest", typeof( AssetBundleManifest ) ) as AssetBundleManifest;
					}
					else
						Develop.LogError( "Error: null == assetBundle" );
				}

				yield return ( null == _manifest );
			}

			static public IEnumerator Patch()
			{
				if( null != _manifest )
				{
					var bundleNames = _manifest.GetAllAssetBundles();
					var patchList = new List<WWW>();

					foreach( var name in bundleNames )
					{
						var code = GetHash( name );
						var url = WwwBundlePath + name;
						var cachedVersion = Caching.IsVersionCached( url, code );

						if( !cachedVersion )
						{
							var www = WWW.LoadFromCacheOrDownload( url, code );
							if( !string.IsNullOrEmpty( www.error ) )
							{
								Develop.LogError( name + ":" + www.error );
								continue;
							}
							patchList.Add( www );

						}
					}

					foreach( var www in patchList )
					{
						if( !www.isDone )
							yield return false;
						_LogDetail( "patched:" + www.url );
					}
				}
				yield return true;
			}

			public static bool DependenciesDoneRecursive( Bundle base_bundle )
			{
				foreach( var item in base_bundle.Dependencies )
				{
					var www = item.BundleWww;
					if( www == null )
					{
						continue;
					}
					if( !www.isDone )
						return false;

					item.BundleObject = www.assetBundle;

					// 					if (!DependenciesDone(item))
					// 						return false;
				}
				return true;
			}

			public static Hash128 GetHash( string asset_bundle_name )
			{
				if( null != _manifest )
					return _manifest.GetAssetBundleHash( asset_bundle_name );
				return Bundle.Empty;
			}

			// Dependencies & Load ABLE Check 
			public static bool DependenciesDone( Bundle base_bundle )
			{
				if( null == _manifest )
					return false;

				//Dependencies Recursive Check
				if( null != base_bundle.Dependencies )
					return DependenciesDoneRecursive( base_bundle );

				//Dependencies Check
				var dependencies = _manifest.GetDirectDependencies( base_bundle.BundleName );
				if( 0 < dependencies.Length )
				{
					//Create Dependencies Bundle
					var first = ( null == base_bundle.Dependencies );
					if( first )
					{
						var count = dependencies.Length;
						base_bundle.Dependencies = new Bundle[count];

						for( int i = 0; i < count; ++i )
						{
							Bundle dependant_bundle;
							if( _Bundle_Dic.TryGetValue( dependencies[i], out dependant_bundle ) )
							{
								base_bundle.Dependencies[i] = dependant_bundle;
							}
							else
							{
								base_bundle.Dependencies[i] = LoadBundleAsync( WwwBundlePath + dependencies[i], dependencies[i] );
							}
						}
					}
					//Develop.Log(base_bundle.BundleName + "LOAD DEPENDENCIES" + dependencies.Length);
					return !first;
				}
				return ( 0 == dependencies.Length );
			}

#if UNITY_EDITOR

			static public T LoadAssetDataBase<T>( string bundle_name, string asset_name ) where T : UObject
			{
				T value = null;
				string strAssetName = System.IO.Path.GetFileNameWithoutExtension( asset_name );
				var assetDependencies = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( bundle_name );
				foreach( var item in assetDependencies )
				{
					string strItemName = System.IO.Path.GetFileNameWithoutExtension( item );
					if( strItemName == strAssetName )
					{
						value = UnityEditor.AssetDatabase.LoadAssetAtPath<T>( item );
						break;
					}
				}
				return value;
			}

			static public T LoadAssetDataBase<T>( string bundle_name, string asset_name, int multple_index ) where T : UObject
			{
				T value = null;
				var assetDependencies = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle( bundle_name );
				foreach( var item in assetDependencies )
				{
					if( item.Contains( asset_name ) )
					{
						if( multple_index <= -1 )
						{
							value = UnityEditor.AssetDatabase.LoadAssetAtPath<T>( item );
						}
						else
						{
							var arrValue = UnityEditor.AssetDatabase.LoadAllAssetsAtPath( item );
							for( int i = 0; i < arrValue.Length; ++i )
							{
								if( arrValue[i].name.EndsWith( "_" + ( multple_index ) ) )
								{
									value = (T)arrValue[i];
								}
							}
						}

						break;
					}
				}
				return value;
			}
#endif
		}
#endif

		#endregion
		#region Constant

#if AUTO_UNLOAD
		private const float _UNLOAD_BUNDLE_LRU_TIME = 5; // 이시간 동안 Close되고 사용 안 한 Bundle은 삭제한다.     
#endif

#if COMMON_DONT_UNLOAD && MANUAL_UNLOAD
		private static HashSet<string> _common_bundles = new HashSet<string>
		{
			"ui_common",
			"skillicon",
			"equipicon",
			"commonicon",
			"uimat",
			"paranshader",
			"uiani",
			"fzltch_gbk",
			"adobeheitistd-regular",

		};
#endif

		static public string BundlePath
		{
			get { return Application.streamingAssetsPath + "/AssetsBundle/"; }
		}
		public static string WwwBundlePath
		{
			get
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				return "file://" + BundlePath;
#elif UNITY_ANDROID
				return BundlePath;
#elif UNITY_IOS
				return "file://" + BundlePath;
#endif
			}
		}


#if UNITY_EDITOR
		private const string _USE_BUNDLE_IN_EDITOR_KEY = "UseBundleInEditor";
#endif

		public static bool UseBundleInEditor
		{
			get
			{
#if UNITY_EDITOR
				return UnityEditor.EditorPrefs.GetBool( _USE_BUNDLE_IN_EDITOR_KEY );
#else
                return true;
#endif
			}

#if UNITY_EDITOR
			set { UnityEditor.EditorPrefs.SetBool( _USE_BUNDLE_IN_EDITOR_KEY, value ); }
#endif
		}


		#endregion
		#region Bundle

		public abstract class Bundle : IEnumerator
		{
			#region Main

			public AssetBundle BundleObject { get; set; }
			private AssetBundleCreateRequest _bundle_request;
			public WWW BundleWww;
			public string BundleName { get; set; }
			public Bundle[] Dependencies;
			public bool DependenciesWait;
			public static readonly Hash128 Empty = new Hash128( 0, 0, 0, 0 );
			protected bool is_unloadall_when_bundleunload = true;
#if UI_RESOURCE
			protected const string texturePath = "UI/Texture/";
#endif
			// Sync load했을 때, 호출되는 생성자
			protected Bundle( AssetBundle bundle_object )
			{
				this.BundleObject = bundle_object;
			}

			// Async load했을 때, 호출되는 생성자
			protected Bundle( AssetBundleCreateRequest bundle_request )
			{
				this._bundle_request = bundle_request;
			}
			protected Bundle( [NotNull] WWW bundle_www )
			{
				this.BundleWww = bundle_www;
			}

			// 设置卸载方式
			public void SetUnloadType( bool unload_true )
			{
				this.is_unloadall_when_bundleunload = unload_true;
			}

			public void Unload( bool full_unload )
			{
#if MANUAL_UNLOAD
				if( this.BundleObject == null )
				{
					return;
				}

                if( _common_bundles.Contains(this.BundleObject.name) )
                {
                    return;
                }

				_Bundle_Dic.Remove( this.BundleObject.name );
				//this.BundleObject.Unload( full_unload );

				if( this.Dependencies == null )
				{
					return;
				}

				for( int i = 0; i < this.Dependencies.Length; i++ )
				{
					if( this.Dependencies[i] == null )
					{
						continue;
					}

					this.Dependencies[i].Unload( full_unload );
					_Bundle_Dic.Remove( this.Dependencies[i].BundleName );
				}
#endif
			}

			public float LoadProgress
			{
				get
				{
					return this.BundleObject != null ? 1
						: this._bundle_request == null ? 0
							: this._bundle_request.progress;
				}
			}

			#endregion
			#region IEnumerator

			void IEnumerator.Reset()
			{

			}

			bool IEnumerator.MoveNext()
			{
				if( null != this.BundleWww )
				{
					if( !this.BundleWww.isDone )
						return true;//yield return this.BundleWww;

					if( !Manifest.DependenciesDone( this ) )
						return this.DependenciesWait = true;//yield return null;

					if( this.BundleWww.error != null )
						Develop.LogError( "Error:" + this.BundleWww.error );

					this.BundleObject = this.BundleWww.assetBundle;
					if( this.BundleObject == null )
						Develop.LogError( "Error: null == BundleObject" );

					this.BundleWww.Dispose();
					this.BundleWww = null;

					return false;
				}

				// 요청된 async load가 없으면 enumerate를 중지한다.
				if( this._bundle_request == null )
					return false;
				Assert.IsNull( this.BundleObject );

				// loading이 완료되지 않았으면, true를 리턴하여, enumerate를 진행한다.
				if( !this._bundle_request.isDone )
					return true;

				// load가 완료되면, object를 얻고 enumerate를 중지한다.
				this.BundleObject = this._bundle_request.assetBundle;
				this._bundle_request = null;

				return false;
			}

			object IEnumerator.Current
			{
				get
				{
					if( !this.BundleWww.isDone )
						return this.BundleWww;//yield return 의 본문은 반복기 블록이 될 수 없습니다.

					if( !this.DependenciesWait )
						return null;

					if( null != this.BundleWww )
						return this.BundleWww;
					// Enumerator.MoveNext()가 true를 리턴했을 때만 이 함수가 호출 되므로, assert 처리한다.
					Assert.IsNotNull( this._bundle_request );
					return this._bundle_request;
				}
			}

			#endregion
			#region Load Asset

			public Asset LoadAsset( [NotNull] string asset_name )
			{
#if UI_RESOURCE
				GameObject asset = ResourcesEx.Load<GameObject>( texturePath + asset_name );
                if (asset == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }

                SpritePrefab sp = asset.GetComponent<SpritePrefab>();
                if (sp == null)
                {
                    Develop.LogError("Sprite Component Missing:" + asset_name);
                    return null;
                }

                return new Asset_Real( this, sp.GetSprite(1));
#else
#if UNITY_EDITOR
				if( !UseBundleInEditor )
				{
					var data_object = Manifest.LoadAssetDataBase<UObject>( this.BundleName, asset_name );
					return new Asset_Real( this, data_object );
				}
#endif
				if( this.BundleObject == null )
				{
					Develop.LogErrorF( "Bundle is not loaded {0}.{1}", this.BundleName, asset_name );
					return null;
				}

				var asset_object = this.BundleObject.LoadAsset( asset_name );
				if( null == asset_object )
				{
					Develop.LogErrorF( "Asset LoadFail {0}.{1}", this.BundleName, asset_name );
					return null;
				}
				return new Asset_Real( this, asset_object );
#endif
			}

			public Asset LoadAssetAsync( [NotNull] string asset_name )
			{
#if UI_RESOURCE
                GameObject asset = ResourcesEx.Load<GameObject>(texturePath + asset_name);
                if (asset == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }

                SpritePrefab sp = asset.GetComponent<SpritePrefab>();
                if (sp == null)
                {
                    Develop.LogError("Sprite Component Missing:" + asset_name);
                    return null;
                }

                Sprite sprite = (Sprite)sp.GetSprite(1);
                if (sprite == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }

                return new Asset_Real( this, sprite);
#else
#if UNITY_EDITOR
				if( !UseBundleInEditor )
				{
					var data_object = Manifest.LoadAssetDataBase<UObject>( this.BundleName, asset_name );
					return new Asset_Real( this, data_object );
				}
#endif
				if( this.BundleObject == null )
				{
					Develop.LogErrorF( "Bundle is not loaded {0}.{1}", this.BundleName, asset_name );
					return null;
				}

				var asset_request = this.BundleObject.LoadAssetAsync( asset_name );
				if( null == asset_request )
				{
					Develop.LogErrorF( "Asset LoadFail {0}.{1}", this.BundleName, asset_name );
					return null;
				}
				return new Asset_Real( this, asset_request );
#endif
			}

			public Asset<T> LoadAsset<T>( [NotNull] string asset_name ) where T : UObject
			{
#if UI_RESOURCE
                GameObject asset = ResourcesEx.Load<GameObject>(texturePath + BundleName + "/" + asset_name);
                if (asset == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }

                SpritePrefab sp = asset.GetComponent<SpritePrefab>();
                if (sp == null)
                {
                    Develop.LogError("Sprite Component Missing:" + asset_name);
                    return null;
                }

                T sprite = (T)sp.GetSprite(asset_name, -1);
                if (sprite == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }
                return new Asset_Real<T>(this, sprite);
#else
#if UNITY_EDITOR
				if( !UseBundleInEditor )
				{
					var data_object = Manifest.LoadAssetDataBase<T>( this.BundleName, asset_name );
					return new Asset_Real<T>( this, data_object );
				}
#endif
				if( this.BundleObject == null )
				{
					Develop.LogErrorF( "Bundle is not loaded {0}.{1}", this.BundleName, asset_name );
					return null;
				}

				if( !this.BundleObject.Contains( asset_name ) )
				{
					Develop.LogErrorF( "Asset CantFind {0}.{1}", this.BundleName, asset_name );
					return null;
				}

				var asset_object = this.BundleObject.LoadAsset<T>( asset_name );
				if( null == asset_object )
				{
					Develop.LogErrorF( "Asset LoadFail {0}.{1}", this.BundleName, asset_name );
					return null;
				}
				return new Asset_Real<T>( this, asset_object );
#endif
            }

			//加载Multiple Sprite
			public Asset<T> LoadAsset<T>( [NotNull] string asset_name, int multple_index ) where T : UObject
			{
#if UI_RESOURCE
                GameObject asset = ResourcesEx.Load<GameObject>(texturePath + asset_name);
                if (asset == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }

                SpritePrefab sp = asset.GetComponent<SpritePrefab>();
                if (sp == null)
                {
                    Develop.LogError("Sprite Component Missing:" + asset_name);
                    return null;
                }

                T sprite = (T)sp.GetSprite(asset_name, -1);
                if(sprite == null)
                {
                    Develop.LogError("Load Error!Cant find sprite:" + asset_name);
                    return null;
                }
                return new Asset_Real<T>( this, sprite);
#else
#if UNITY_EDITOR
				if( !UseBundleInEditor )
				{
					var data_object = Manifest.LoadAssetDataBase<T>( this.BundleName, asset_name, multple_index );
					return new Asset_Real<T>( this, data_object );
				}
#endif
				if( this.BundleObject == null )
				{
					Develop.LogErrorF( "Bundle is not loaded {0}.{1}, Index = {2}", this.BundleName, asset_name, multple_index );
					return null;
				}

				if( !this.BundleObject.Contains( asset_name ) )
				{
					Develop.LogErrorF( "Asset Multiple Sprite CantFind {0}.{1}, Index = {2}", this.BundleName, asset_name, multple_index );
					return null;
				}

				var asset_object = this.BundleObject.LoadAssetWithSubAssets<T>( asset_name );
				if( null == asset_object )
				{
					Develop.LogErrorF( "Asset Multiple Sprite CantFind {0}.{1}, Index = {2}", this.BundleName, asset_name, multple_index );
					return null;
				}
				if( asset_object.Length > multple_index )
				{
					return new Asset_Real<T>( this, asset_object[multple_index] );
				}

				return null;
#endif
			}

			public Asset<T> LoadAssetAsync<T>( [NotNull] string asset_name ) where T : UObject
			{
#if UNITY_EDITOR
				if( !UseBundleInEditor )
				{
					var data_object = Manifest.LoadAssetDataBase<T>( this.BundleName, asset_name );
					return new Asset_Real<T>( this, data_object );
				}
#endif
				if( this.BundleObject == null )
				{
					Develop.LogErrorF( "Bundle is not loaded {0}.{1}", this.BundleName, asset_name );
					return null;
				}

				var asset_request = this.BundleObject.LoadAssetAsync( asset_name );
				if( null == asset_request )
				{
					Develop.LogErrorF( "Asset LoadFail {0}.{1}", this.BundleName, asset_name );
					return null;
				}

				return new Asset_Real<T>( this, asset_request );
			}

			#endregion
		}

		// 외부에는 Bundle class만 공개하고, 생성만 담당하는 Bundle_Real class에 담고 있으면, public setter를 외부에 공개하지 않아도 된다.
		private sealed class Bundle_Real : Bundle
		{
			// Sync load했을 때, 호출되는 생성자
			public Bundle_Real( [NotNull] AssetBundle bundle_object, string bundle_name )
				: base( bundle_object )
			{
				_opened_bundle_count++;
				this.BundleName = bundle_name;
			}

			// Async load했을 때, 호출되는 생성자
			public Bundle_Real( [NotNull] AssetBundleCreateRequest bundle_request, string bundle_name )
				: base( bundle_request )
			{
				_opened_bundle_count++;
				this.BundleName = bundle_name;
			}
			// WWW
			public Bundle_Real( [NotNull] WWW bundle_www, string bundle_name )
				: base( bundle_www )
			{
				_opened_bundle_count++;
				this.BundleName = bundle_name;
				_LogDetail( _opened_bundle_count + "Created" + this.BundleName );
			}

#if UI_RESOURCE || UNITY_EDITOR
			public Bundle_Real( string bundle_name )
				: base( null as AssetBundle )
			{
				this.BundleName = bundle_name;
			}
#endif

			~Bundle_Real()
			{
				_LogDetail( _opened_bundle_count + "Distroyed" + this.BundleName );

				_opened_bundle_count--;
				//this.BundleObject.Unload(is_unloadall_when_bundleunload);
				//_closed_bundle.Enqueue(this.BundleObject);
#if AUTO_UNLOAD
				if( null != this.BundleWww )
				{
					_closed_www.Enqueue( this.BundleWww );
				}
#endif
			}
		}


		/// <summary>
		/// 获取一个AssetBundle
		/// </summary>
		/// <param name="bundle_name"></param>
		/// <returns></returns>
		public static Bundle GetAssetBundle( string bundle_name )
		{
			Bundle bundle;
			_Bundle_Dic.TryGetValue( bundle_name, out bundle );
			return bundle;
		}

		/// <summary>
		/// 从资源包里加载资源，资源包和资源名相同
		/// </summary>
		/// <param name="asset_name"></param>
		/// <returns></returns>
		public static UObject LoadAssetFromBundle( string asset_name )
		{
			string bundle_name = asset_name.ToLower();
			Asset asset;
#if UNITY_EDITOR
			if( !UseBundleInEditor )
			{
				var br = new Bundle_Real( bundle_name );
				asset = br.LoadAsset( asset_name );

				return asset.Object;
			}
#endif
			Bundle bundle;
			if( !_Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				Develop.LogError( "Can't find bundle :" + bundle_name );
				return null;
			}

			asset = bundle.LoadAsset( bundle_name );
			if( asset == null )
			{
				Develop.LogError( "Can't find asset :" + bundle_name );
				return null;
			}
			return asset.Object;
		}

		/// <summary>
		/// 从资源包里加载资源，资源包和资源名相同
		/// </summary>
		/// <param name="asset_name"></param>
		/// <returns></returns>
		public static T LoadAssetFromBundle<T>( string asset_name ) where T : UObject
		{
			string bundle_name = asset_name.ToLower();
			Asset<T> asset;
#if UNITY_EDITOR
			if( !UseBundleInEditor )
			{
				var br = new Bundle_Real( bundle_name );
				asset = br.LoadAsset<T>( asset_name );

				return asset.Object;
			}
#endif
			Bundle bundle;
			if( !_Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				Develop.LogError( "Can't find bundle :" + bundle_name );
				return null;
			}

			asset = bundle.LoadAsset<T>( bundle_name );
			if( asset == null )
			{
				Develop.LogError( "Can't find asset :" + bundle_name );
				return null;
			}
			return asset.Object;
		}

		/// <summary>
		/// 同步加载一个资源，暂时不使用
		/// </summary>
		/// <param name="asset_name"></param>
		/// <returns></returns>
		public static UObject SyncLoadAsset( string asset_name )
		{
			string bundle_path = BundlePath + asset_name.ToLower();
			Bundle bundle;
			if( !_Bundle_Dic.TryGetValue( asset_name, out bundle ) )
			{
				bundle = LoadBundle( bundle_path );
			}

			if( bundle == null )
			{
				Develop.LogError( "Load Bundle Failed! BundleName:" + asset_name );
				return null;
			}

			Asset asset = bundle.LoadAsset( asset_name );
			if( asset == null )
			{
				Develop.LogError( "Load Asset Failed! AssetName:" + asset_name );
				return null;
			}

			return asset.Object;
		}

		public static IEnumerator Enumerate_AsyncLoad( string bundle_name )
		{
#if UI_RESOURCE
			yield return LoadBundle( bundle_name );
#else
			bundle_name = bundle_name.ToLower();
			Bundle bundle;
			if( _Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				while( bundle.BundleObject == null )
					yield return null;
				yield return bundle;
			}

			string bundle_path = WwwBundlePath + bundle_name;
			bundle = LoadBundleAsync( bundle_path, bundle_name );
			yield return bundle;
#endif
		}

		public static Bundle LoadBundle( [NotNull] string bundle_path ) //to test legacy code 
		{
#if UI_RESOURCE
			return new Bundle_Real( bundle_path );
#else
			var name = System.IO.Path.GetFileName( bundle_path );

#if UNITY_EDITOR
			if( !UseBundleInEditor )
				return new Bundle_Real( name );
#endif
			Bundle bundle;
			if( _Bundle_Dic.TryGetValue( name, out bundle ) )
			{
				return bundle;
			}
			return bundle_path.Contains( WwwBundlePath )
				? _LoadBundleAsyncWww( bundle_path, name )
				: _LoadBundleFile( bundle_path, name );
#endif
		}

		public static Bundle LoadBundleAsync( [NotNull] string bundle_path, string inner_path = null/*assetBundle路径下的子目录*/)
		{
			if( string.IsNullOrEmpty( inner_path ) )
			{
				inner_path = bundle_path;
			}

			var name = inner_path;//System.IO.Path.GetFileName(bundle_path);

#if UNITY_EDITOR
			if( !UseBundleInEditor )
				return new Bundle_Real( name );
#endif
			Bundle bundle;
			if( _Bundle_Dic.TryGetValue( name, out bundle ) )
			{
				return bundle;
			}

			return !bundle_path.Contains( WwwBundlePath )
				? _LoadBundleAsyncFile( bundle_path, name )
				: _LoadBundleAsyncWww( bundle_path, name );
		}

		private static Bundle _LoadBundleAsyncWww( string bundle_path, string bundle_name )
		{
			Hash128 code = Manifest.GetHash( bundle_name );
			return LoadBundleAsyncWww( bundle_path, bundle_name, code );
		}

		public static Bundle LoadBundleAsyncWww( string bundle_path, string bundle_name, Hash128 code )
		{
			Bundle bundle;
			if( _Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				return bundle;
			}
			var cachedVersion = Caching.IsVersionCached( bundle_path, code );

			var bundle_www = cachedVersion ? WWW.LoadFromCacheOrDownload( bundle_path, code ) : new WWW( bundle_path );

#if !AFTER_CHECK
			var after_cacheAble = Caching.IsVersionCached( bundle_path, code );
			if( cachedVersion != after_cacheAble )
			{
				Develop.LogError( cachedVersion + "Cached:" + bundle_path );
			}
#endif

			if( !string.IsNullOrEmpty( bundle_www.error ) )
				Develop.LogError( "Load FAIL" + bundle_path + ":" + bundle_www.error );

			var real = new Bundle_Real( bundle_www, bundle_name );
			_Bundle_Dic.Add( bundle_name, real );
			return real;
		}

		private static Bundle _LoadBundleFile( string bundle_path, string bundle_name )
		{
			Bundle bundle;
			if( _Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				return bundle;
			}
			var bundle_object = AssetBundle.LoadFromFile( bundle_path );

			if( null == bundle_object )
			{
				Develop.LogError( "Load FAIL" + bundle_path );
				return null;
			}

#if AUTO_UNLOAD
			if( _destroying_coroutine == null )
				_destroying_coroutine = Main.Behaviour.StartCoroutine( _UnloadClosedBundle() );
#endif

			var real = new Bundle_Real( bundle_object, bundle_name );
			return real;
		}

		private static Bundle _LoadBundleAsyncFile( string bundle_path, string bundle_name )
		{
			Bundle bundle;
			if( _Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				return bundle;
			}
			var bundle_request = AssetBundle.LoadFromFileAsync( bundle_path );
			if( null == bundle_request )
			{
				Develop.LogError( "Load FAIL" + bundle_path );
				return null;
			}

#if AUTO_UNLOAD
			if( _destroying_coroutine == null )
				_destroying_coroutine = Main.Behaviour.StartCoroutine( _UnloadClosedBundle() );
#endif

			var real = new Bundle_Real( bundle_request, bundle_name );
			return real;
		}

		#endregion
		#region Asset class

		public abstract class AssetBase : IEnumerator
		{
			#region Main

			[NotNull]
			public readonly Bundle Bundle;

			private AssetBundleRequest _asset_request;
			protected abstract UObject asset_object { get; set; }

			protected AssetBase( [NotNull] Bundle bundle_object )
			{
				this.Bundle = bundle_object;
			}

			protected AssetBase( [NotNull] Bundle bundle_object, [NotNull] AssetBundleRequest asset_request )
			{
				this.Bundle = bundle_object;
				this._asset_request = asset_request;
			}

			public float LoadProgress
			{
				get
				{
					return this.asset_object != null ? 1
						: this._asset_request == null ? 0
							: this._asset_request.progress;
				}
			}

			#endregion
			#region IEnumerator

			void IEnumerator.Reset()
			{
			}

			bool IEnumerator.MoveNext()
			{
				// 요청된 async load가 없으면 enumerate를 중지한다.
				if( this._asset_request == null )
					return false;
				Assert.IsNull( this.asset_object );

				// loading이 완료되지 않았으면, true를 리턴하여, enumerate를 진행한다.
				if( !this._asset_request.isDone )
					return true;

				// load가 완료되면, object를 얻고 enumerate를 중지한다.
				this.asset_object = this._asset_request.asset;
				this._asset_request = null;
				return false;
			}


			object IEnumerator.Current
			{
				get
				{
					// Enumerator.MoveNext()가 true를 리턴했을 때만 이 함수가 호출 되므로, assert 처리한다.
					Assert.IsNotNull( this._asset_request );
					return this._asset_request;
				}
			}

			#endregion
		}

		public abstract class Asset : AssetBase
		{
			protected override sealed UObject asset_object { get; set; }

			public UObject Object
			{
				get { return this.asset_object; }
			}

			protected Asset( [NotNull] Bundle bundle_object, [NotNull] UObject asset_object )
				: base( bundle_object )
			{
				this.asset_object = asset_object;
			}

			protected Asset( [NotNull] Bundle bundle_object, [NotNull] AssetBundleRequest asset_request )
				: base( bundle_object, asset_request )
			{
			}
		}

		public abstract class Asset<T> : AssetBase where T : UObject
		{
			protected override sealed UObject asset_object
			{
				get { return this.Object; }
				set { this.Object = (T)value; }
			}

			public T Object { get; private set; }

			protected Asset( [NotNull] Bundle bundle_object, [NotNull] T asset_object )
				: base( bundle_object )
			{
				this.Object = asset_object;
			}

			protected Asset( [NotNull] Bundle bundle_object, [NotNull] AssetBundleRequest asset_request )
				: base( bundle_object, asset_request )
			{
			}
		}

		// 외부에는 Asset class만 공개하고, 생성만 담당하는 Asset_Real class에 담고 있으면, public setter를 외부에 공개하지 않아도 된다.
		private sealed class Asset_Real : Asset
		{
			public Asset_Real( [NotNull] Bundle bundle_object, [NotNull] UObject asset_object )
				: base( bundle_object, asset_object )
			{
			}

			public Asset_Real( [NotNull] Bundle bundle_object, [NotNull] AssetBundleRequest asset_request )
				: base( bundle_object, asset_request )
			{
			}
		}

		private sealed class Asset_Real<T> : Asset<T> where T : UObject
		{
			public Asset_Real( [NotNull] Bundle bundle_object, [NotNull] T asset_object )
				: base( bundle_object, asset_object )
			{
			}

			public Asset_Real( [NotNull] Bundle bundle_object, [NotNull] AssetBundleRequest asset_request )
				: base( bundle_object, asset_request )
			{
			}
		}

		#endregion
		#region Auto Unload

#if AUTO_UNLOAD
		private static readonly WaitForSeconds _wait_next_close_time = new WaitForSeconds( _UNLOAD_BUNDLE_LRU_TIME );
		private static Coroutine _destroying_coroutine;
		private static readonly ConcurrentQueue<AssetBundle> _closed_bundle = new ConcurrentQueue<AssetBundle>();
		private static readonly ConcurrentQueue<AssetBundle> _closed_bundle_unload_false = new ConcurrentQueue<AssetBundle>();
		private static readonly ConcurrentQueue<WWW> _closed_www = new ConcurrentQueue<WWW>();
#endif
		private static readonly Dictionary<string, Bundle> _Bundle_Dic = new Dictionary<string, Bundle>();
		private static int _opened_bundle_count;

		/// <summary>
		/// 卸载一个资源包
		/// </summary>
		/// <param name="bundle_name"></param>
		/// <param name="full_release">
		/// True:完全释放并且释放引用，False:只释放资源包
		/// </param>
		public static void UnloadBundle( string bundle_name, bool full_release )
		{
#if MANUAL_UNLOAD
			bundle_name = bundle_name.ToLower();

#if COMMON_DONT_UNLOAD
			if( _common_bundles.Contains( bundle_name ) )
			{
				return;
			}
#endif

			Bundle bundle;
			if( !_Bundle_Dic.TryGetValue( bundle_name, out bundle ) )
			{
				return;
			}

			bundle.Unload( full_release );
			_Bundle_Dic.Remove( bundle_name );
#endif
		}

		/// <summary>
		/// 卸载一组资源包
		/// </summary>
		/// <param name="bundle_liast"></param>
		/// <param name="full_release">
		/// True:完全释放并且释放引用，False:只释放资源包
		/// </param>
		public static void UnloadBundle( string[] bundle_liast, bool full_release )
		{
			for( int i = 0; i < bundle_liast.Length; i++ )
			{
				UnloadBundle( bundle_liast[i], full_release );
			}
		}

		/// <summary>
		/// 卸载一组资源包
		/// </summary>
		/// <param name="bundle_list"></param>
		/// <param name="full_release">
		/// True:完全释放并且释放引用，False:只释放资源包
		/// </param>
		public static void UnloadBundle( List<string> bundle_list, bool full_release )
		{
			for( int i = 0; i < bundle_list.Count; i++ )
			{
				UnloadBundle( bundle_list[i], full_release );
			}
		}

#if AUTO_UNLOAD
		private static IEnumerator _UnloadClosedBundle()
		{
			while( true )
			{
				// close된 bundle이 있을 때까지 while문을 돌린다.
				while( _closed_bundle.IsEmpty && _closed_bundle_unload_false.IsEmpty )
				{
					// 아직 close 된 bundle없어도, load된 bundle이 있으면, 계속 대기한다.
					if( _opened_bundle_count > 0 )
					{
						yield return _wait_next_close_time;
					}
					// load된 bundle이 하나도 없으면, coroutine을 종료한다.
					else
					{
						_destroying_coroutine = null;
						yield break;
					}
				}

				// close된 stream을 순회하며, 제거한다.
				AssetBundle bundle_object;
				if( !_closed_bundle.IsEmpty )
				{
					while( _closed_bundle.TryDequeue( out bundle_object ) )
					{
						_LogDetail( _opened_bundle_count + " Unload " + bundle_object.name );
						bundle_object.Unload( true ); // 이 bundle을 사용하는 모든 asset이 이미 제거된 상태이므로 true 인자로 unload할 수 있다.
					}
				}
				if( !_closed_bundle_unload_false.IsEmpty )
				{
					while( _closed_bundle_unload_false.TryDequeue( out bundle_object ) )
					{
						_LogDetail( _opened_bundle_count + " Unload " + bundle_object.name );
                        if(bundle_object == null)
                        {
                            continue;
                        }
						bundle_object.Unload( false );
					}
				}

				WWW bundle_www;
				while( _closed_www.TryDequeue( out bundle_www ) )
				{
					if( null != bundle_www )
					{
						_LogDetail( _opened_bundle_count + "WWWUnload" );
						bundle_www.Dispose();
					}
				}

			}
		}
#endif

		#endregion
		#region for Editor

#if UNITY_EDITOR
		public static Bundle GetEditorBundle( string bundle_name )
		{
			return new Bundle_Real( bundle_name );
		}
#endif

		// Shader Reassign for using Bundle in Editor
		[Conditional( "UNITY_EDITOR" )]
		public static void ReassignShader( UObject o )
		{
#if UNITY_EDITOR
			// shader
			if( o is Shader )
				return;

			// material
			var material = o as Material;
			if( material )
			{
				int render_queue = material.renderQueue;
				material.shader = Shader.Find( material.shader.name );
				material.renderQueue = render_queue;
				return;
			}

			// renderer
			var renderer = o as Renderer;
			if( renderer )
			{
				var materials = renderer.sharedMaterials;
				for( int i = 0; i < materials.Length; i++ )
				{
					var m = materials[i];
					int render_queue = m.renderQueue;
					m.shader = Shader.Find( m.shader.name );
					m.renderQueue = render_queue;
				}
				return;
			}

			var image = o as Image;
			if( image )
			{
				image.material.shader = Shader.Find( image.material.shader.name );
			}

			// entity
			var entity = o as Entity;
			if( entity )
			{
				var components = entity.GetComponentsInChildren<Component>( true );
				for( int i = 0; i < components.Length; i++ )
				{
					ReassignShader( components[i] );
				}
				return;
			}

			// 일반 객체
			var so = new UnityEditor.SerializedObject( o );
			var sp = so.GetIterator();
			while( sp.NextVisible( true ) )
			{
				if( sp.propertyType != UnityEditor.SerializedPropertyType.ObjectReference )
					continue;
				if( sp.objectReferenceValue == null )
					continue;

				var shader = sp.objectReferenceValue as Shader;
				if( shader == null )
					continue;

				sp.objectReferenceValue = Shader.Find( shader.name );
			}
#endif
		}

#if SHOW_DETAIL_LOG
	    private static void _LogDetail( string message )
	    {
		    Develop.Log( message );
	    }
#else
		[Conditional( "NOT_USED" )]
		// ReSharper disable once UnusedParameter.Local
		private static void _LogDetail( string message )
		{
		}
#endif

		#endregion
	}
}