var TemplateController = (function () {
    return {
        MessagesTemplates: {
            myTextMessage: (message, date) => {
                if (date)
                    date = moment.utc(date).local().format('YYYY-MM-DD HH:mm:ss a');
                else
                    date = moment().format('h: mm: ss a');
                return ` <li class="sent">
                        <p>${message}</p>
                    </li>
                  `;
            },
            friendTextMessage: (message, date) => {
                if (date)
                    date = moment.utc(date).local().format('YYYY-MM-DD HH:mm:ss a');
                else
                    date = moment().format('h: mm: ss a');
                return `
                <li class="replies">
                        <p>${message}</p>
                    </li>
                  `;

            },
            myImageMessage: (message, date) => {
                if (date)
                    date = moment.utc(date).local().format('YYYY-MM-DD HH:mm:ss a');
                else
                    date = moment().format('h: mm: ss a');

                return `
      
<li class="sent">
                      <img class="chat_img"  src=" ${message}">
                    </li>
        `;
            },
            friendImageMessage: (message, date) => {
                if (date)
                    date = moment.utc(date).local().format('YYYY-MM-DD HH:mm:ss a');
                else
                    date = moment().format('h: mm: ss a');
                return `
               <li class="replies">
                      <img class="chat_img"  src=" ${message}">
                    </li>
        `;

            },
            serverTextMessage: (message) => {
                return `
                 <li class="server">
                        <p>${message}</p>
                    </li>
                </div>
                 `;

            },
        }
    }
})();