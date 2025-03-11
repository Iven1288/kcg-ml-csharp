using Shared;
using System.CommandLine;
using System.CommandLine.Invocation;
using Settings = NanoGptSetting.Settings;
using TorchSharp;
using LogUtility;
using FileLib;

namespace NanoGptArgsSettings
{
    public class ArgsSettings
    {
        public string       trainingDataset = "../nano-gpt/input.txt";
        public string       modelOutputDirectory = "C:/Models/";
        public Mode         Mode = Mode.Train;
        public torch.Device Device = torch.CPU;

        public string SaveLocation(int vocabSize) => $"{this.modelOutputDirectory}/NanoGpt_{Device.type}_{Settings.NEmbed}_{Settings.NHead}_{Settings.NLayer}_{vocabSize}.dat";

        public ArgsSettings(string[] args)
        {
            ParseCommandLineArguments(args);
        }

        public void ParseCommandLineArguments(string[] args)
        {
            Option<string> runningModeOption = new Option<string>(
                "--running-mode",
                "Let NanoGpt run in <inference | training> mode, training by default"
            );

            Option<string> trainDatasetOption = new Option<string>(
                "--training-dataset",
                "Absolute path of input file for training"
            );

            Option<string> modelOutputDirOption = new Option<string>(
                "--model-output-directory",
                "Absolute path of directory containing NanoGpt weight data"
            );

            Option<string> nanoGptDeviceOption = new Option<string>(
                "--device",
                "Set NanoGpt run on CPU or GPU, <CPU | CUDA>, CPU by default"
            );

            RootCommand rootCommand = new RootCommand("NanoGpt app")
            {
                runningModeOption,
                trainDatasetOption,
                modelOutputDirOption,
                nanoGptDeviceOption
            };

            rootCommand.Handler = CommandHandler.Create<string, string, string, string>(
                (string runningMode, string trainingDataset, string modelOutputDirectory, string device) =>
                {
                    // setting running mode
                    if (runningMode.Equals("inference"))
                    {
                        this.Mode = Mode.Generate;
                        LibLog.LogInfo("Set NanoGPT to run in inference mode");
                    }
                    else if (runningMode.Equals("training"))
                    {
                        this.Mode = Mode.Train;
                        LibLog.LogInfo("Set NanoGPT to run in training mode");
                    }
                    else
                    {
                        this.Mode = Mode.Train;
                        LibLog.LogInfo("Invalid parameter for --running-mode, defaulting to training mode");
                    }

                    // setting training data path
                    if (!string.IsNullOrEmpty(trainingDataset))
                    {
                        this.trainingDataset = trainingDataset;
                    }
                    LibLog.LogInfo($"Training dataset: {this.trainingDataset}");

                    // setting weight data storage dir
                    if (!string.IsNullOrEmpty(modelOutputDirectory))
                    {
                        this.modelOutputDirectory = modelOutputDirectory;
                    }
                    LibLog.LogInfo($"Model output directory: {this.modelOutputDirectory}");

                    // setting device
                    if (device.Equals("CUDA"))
                    {
                        if(torch.cuda.is_available()){
                            this.Device = torch.CUDA;
                            LibLog.LogInfo("Set NanoGPT to run on CUDA");
                        }
                        else
                        {
                            this.Device = torch.CPU;
                            LibLog.LogInfo("CUDA is not available, set NanoGPT to run on CPU");
                        }
                    }
                }
            );

            rootCommand.InvokeAsync(args);
        }
    }
}