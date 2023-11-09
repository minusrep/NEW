mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  RateGameExtern: function () {
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
          gameInstance.SendMessage('YandexSDK', 'OnRateGame');
          console.log(feedbackSent);
        })
      } else {
        console.log(reason)
      }
    })
  },

  SaveDataExtern: function (data) {
    var dataString = UTF8ToString(data);
    var myobj = JSON.parse(dataString);
    player.setData(myobj).then(() => {
      console.log('SUCCSES: INDEX');
    }).catch(() => {
      console.log('error')});
  },

  LoadDataExtern: function () {

    YaGames
    .init()
    .then(ysdk => {
      ysdk.getPlayer().then(_player => {
        _player.getData().then(_data => {
          const myJSON = JSON.stringify(_data);
          gameInstance.SendMessage('[GAME MANAGER]','SetData', myJSON);
/*          var lang = ysdk.environment.i18n.lang;
          var bufferSize = lengthBytesUTF8(lang) + 1;
          var buffer = _malloc(bufferSize);
          stringToUTF8(lang, buffer, bufferSize);
          console.log('LANGuAGE DONE');
          gameInstance.SendMessage('[GAME MANAGER]','SetLanguage', buffer);*/

        });
      })
    });
  },

  ShowFullscreenAdvExtern: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          gameInstance.SendMessage('YandexSDK', 'OnClose');
        },
        onError: function(error) {
        // some action on error
        }
      }
    })
  },

  ShowRewardedAdvExtern: function(){
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          console.log('Rewarded!');
          gameInstance.SendMessage('YandexSDK', 'OnAdvRewarded');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },


  UpdateLeaderboardExtern: function (value) {
    ysdk.getLeaderboards()
    .then(lb => {
      lb.setLeaderboardScore('Sample', value);
    });

  },

  GetLanguageExtern: function () {
    console.log('GOT LANGUAGE');
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;

  },

  ShowRewardToMoneyAdvExtern: function(){
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          console.log('Rewarded!');
          gameInstance.SendMessage('YandexSDK', 'OnWatchRewardToMoneyAdv');
        },
        onClose: () => {
          console.log('Video ad closed.');
          gameInstance.SendMessage('YandexSDK', 'OnClose');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  ShowRewardToContinueAdvExtern: function(){
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          console.log('Rewarded!');
          gameInstance.SendMessage('YandexSDK', 'OnWatchRewardToContinueAdv');
        },
        onClose: () => {
          console.log('Video ad closed.');
          gameInstance.SendMessage('YandexSDK', 'OnClose');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },


});